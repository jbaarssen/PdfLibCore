using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CppSharp;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using PdfLibCore.CppSharp.Helpers;
using PdfLibCore.CppSharp.Models;
using Serilog;
using Version = System.Version;

namespace PdfLibCore.CppSharp;

public partial class Runner
{
    private readonly ILogger _logger;
    private readonly RunnerOptions _options;
    private readonly HttpClient _httpClient = new();

    public Runner(ILogger logger, RunnerOptions options = null)
    {
        _logger = logger;
        _options = options ?? new RunnerOptions();
    }

    public async Task RunAsync()
    {
        _logger.Information("Downloading PDFium release info from {Url}", _options.GithubDownloadUrl);
        _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0;)");
        var releaseInfo = await _httpClient.GetFromJsonAsync<Release>(_options.GithubDownloadUrl);

        _logger.Information("Downloaded. Reading PDFium release info");
        var versionTag = Version.Parse(releaseInfo!.Name.Split(" ")[1]);
        var version = new Version(
            versionTag.Major,
            versionTag.Minor,
            versionTag.Build,
            _options.BuildRevision == 0 ? versionTag.Revision : _options.BuildRevision);

        _logger.Information("Complete. Using version {Version}", version);

        if (Directory.Exists(_options.DestinationLibraryPath))
        {
            Directory.Delete(_options.DestinationLibraryPath, true);
        }
        Directory.CreateDirectory(_options.DestinationLibraryPath);

        var win64Info = await DownloadAndExtract(new LibraryInfo(_options.DestinationLibraryPath, releaseInfo));

        if (_options.BuildBindings)
        {
            // Build PDFium.cs from the windows x64 build header files.
            var current = Console.Out;
            await using var consoleWriter = new StringWriter();

            try
            {
                Console.SetOut(consoleWriter);
                ConsoleDriver.Run(new Library(win64Info.ExtractedLibBaseDirectory, _options));
            }
            finally
            {
                Console.SetOut(current);
                var lines = consoleWriter.GetStringBuilder().ToString().Split(Environment.NewLine).Where(s => !string.IsNullOrWhiteSpace(s));
                foreach (var line in lines)
                {
                    var match = ValueRegex().Match(line);
                    // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                    _logger.Information(match.Success
                        ? line.Replace(match.Groups[1].Value, "{Value}")
                        : line, match.Groups[1].Value);
                }
            }

            if (Directory.Exists(_options.PdfiumGeneratedProjectDir))
            {
                foreach (var file in Directory.GetFiles(_options.PdfiumGeneratedProjectDir, "*.cs", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }
            }
            Directory.CreateDirectory(_options.PdfiumGeneratedProjectDir);

            await CopyAsync("Marshallers", "Types", "Interfaces");

            await CreateAsync("Pdfium.cs", $@"// ReSharper disable once CheckNamespace
namespace PdfLibCore.Generated;

public static partial class {_options.SharedLibraryName}
{{
    public static bool IsAvailable {{ get; }}

	static {_options.SharedLibraryName}()
	{{
		IsAvailable = Initialize();
	}}

	private static bool Initialize()
	{{
		try
		{{
			FPDF_InitLibrary();
		}}
		catch
		{{
			return false;
		}}
		return true;
	}}
}}");

            var generatedCsPaths = Directory.GetFiles(win64Info.ExtractedLibBaseDirectory, "fpdf*.cs");
            foreach (var generatedCsPath in generatedCsPaths)
            {
                // Add the additional build information in the header.
                var fileContents = await File.ReadAllTextAsync(generatedCsPath);

                fileContents = CommentHelper.Replace(fileContents);
                fileContents = fileContents.Replace(
                    $"public unsafe partial class {_options.SharedLibraryName}",
                    $"public static unsafe partial class {_options.SharedLibraryName}");

                var fileName = Path.GetFileName(generatedCsPath);
                _logger.Information("Copying '{File}'", fileName);
                fileName = fileName.Replace("fpdf", "FPDF").Replace("_", string.Empty);
                fileName = $"{fileName[..4]}{char.ToUpper(fileName[4])}{fileName[5..]}";

                await using var fs = new FileStream(Path.GetFullPath(Path.Combine(_options.PdfiumGeneratedProjectDir, $"{_options.SharedLibraryName}.{fileName}")), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                await using var sw = new StreamWriter(fs);
                await sw.WriteLineAsync($"// Built from precompiled binaries at {releaseInfo.HtmlUrl}");
                await sw.WriteLineAsync($"// Github release api {releaseInfo.Url}");
                await sw.WriteLineAsync($"// PDFium version v{versionTag} {releaseInfo.TagName} [{releaseInfo.TargetCommitish}]");
                await sw.WriteLineAsync($"// Built on: {DateTimeOffset.UtcNow:R}");
                await sw.WriteLineAsync(string.Empty);
                await sw.WriteLineAsync("// ReSharper disable all");
                await sw.WriteLineAsync("#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type");
                await sw.WriteLineAsync(string.Empty);
                await sw.WriteAsync(fileContents);
            }
        }

        _logger.Information("Done");
    }

    private async Task CreateAsync(string filename, string content)
    {
        Directory.CreateDirectory(_options.PdfiumGeneratedProjectDir);
        await File.WriteAllTextAsync(Path.Combine(_options.PdfiumGeneratedProjectDir, filename), content);
    }

    private async Task CopyAsync(params string[] namespaces)
    {
        foreach (var ns in namespaces)
        {
            Directory.CreateDirectory(Path.Combine(_options.PdfiumGeneratedProjectDir, ns));

            var names = GetType().Assembly.GetManifestResourceNames().Where(name => name.Contains(ns));
            foreach (var fullName in names)
            {
                var file = string.Join('.', fullName.Split('.').Reverse().Take(2).Reverse());

                _logger.Information("Copying {File}", Path.GetFileName(fullName));

                var data = await BinaryData.FromStreamAsync(GetType().Assembly.GetManifestResourceStream(fullName)!);
                await File.WriteAllBytesAsync(Path.Combine(_options.PdfiumGeneratedProjectDir, ns, file), data.ToArray());
            }
        }
    }

    private async Task<LibraryInfo> DownloadAndExtract(LibraryInfo libraryInfo)
    {
        var filename = Path.GetFileName(libraryInfo.BrowserDownloadUrl.LocalPath);

        _logger.Information("Downloading {Filename}...", filename);

        var fullFilePath = Path.Combine(libraryInfo.ExtractedLibBaseDirectory, filename);
        await File.WriteAllBytesAsync(fullFilePath, await _httpClient.GetByteArrayAsync(libraryInfo.BrowserDownloadUrl));

        _logger.Information("Download Complete. Unzipping...");

        if (".zip".Equals(Path.GetExtension(filename), StringComparison.InvariantCultureIgnoreCase))
        {
            new FastZip().ExtractZip(fullFilePath, libraryInfo.ExtractedLibBaseDirectory, null);
        }
        else
        {
            await using var fileStream = File.OpenRead(fullFilePath);
            await using var inputStream = new GZipInputStream(fileStream);
            using var archive = TarArchive.CreateInputTarArchive(inputStream, Encoding.UTF8);
            archive.ExtractContents(libraryInfo.ExtractedLibBaseDirectory);
        }

        _logger.Information("Unzip complete");

        return libraryInfo;
    }

    [GeneratedRegex("'([^']*)")]
    private static partial Regex ValueRegex();
}