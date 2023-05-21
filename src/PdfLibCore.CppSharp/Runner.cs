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

        _logger.Information("Reading PDFium release info");
        var versionTag = Version.Parse(releaseInfo!.Name.Split(" ")[1]);
        var version = new Version(
            versionTag.Major,
            versionTag.Minor,
            versionTag.Build,
            _options.BuildRevision == 0 ? versionTag.Revision : _options.BuildRevision);

        _logger.Information("Using version {Version}", version);

        var latestVersionUsed = await File.ReadAllTextAsync(Path.Combine(_options.SolutionDir, "pdfium-version.txt"));
        using var libraryInfo = new LibraryInfo(_options.DestinationLibraryPath, releaseInfo);
        if (string.IsNullOrWhiteSpace(latestVersionUsed) || !latestVersionUsed.Equals(version.ToString()))
        {
            await File.WriteAllTextAsync(Path.Combine(_options.SolutionDir, "pdfium-version.txt"), version.ToString());
        }
        await DownloadAsync(libraryInfo);
        await ExtractAsync(libraryInfo);

        // Build PDFium.cs from the windows x64 build header files.
        var current = Console.Out;
        await using var consoleWriter = new StringWriter();

        try
        {
            Console.SetOut(consoleWriter);
            ConsoleDriver.Run(new Library(libraryInfo.ExtractedLibBaseDirectory, _options));
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

        await CopyResourcesAsync("Marshalers", "Types", "Interfaces");

        var generatedCsPaths = Directory.GetFiles(libraryInfo.ExtractedLibBaseDirectory, "fpdf*.cs");
        foreach (var generatedCsPath in generatedCsPaths)
        {
            var fileName = Path.GetFileName(generatedCsPath);
            _logger.Information("Copying '{File}'", fileName);
            fileName = fileName.Replace("fpdf", "FPDF").Replace("_", string.Empty);
            fileName = $"{fileName[..4]}{char.ToUpper(fileName[4])}{fileName[5..]}";

            await using var fs = new FileStream(Path.GetFullPath(Path.Combine(_options.PdfiumGeneratedProjectDir, $"{_options.SharedLibraryName}.{fileName}")), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            await using var sw = new StreamWriter(fs);
            await sw.WriteLineAsync($"// Built from precompiled binaries at {releaseInfo.HtmlUrl}");
            await sw.WriteLineAsync($"// Github release api {releaseInfo.Url}");
            await sw.WriteLineAsync($"// PDFium version v{versionTag} {releaseInfo.TagName} [{releaseInfo.TargetCommitish}]");
            await sw.WriteLineAsync(string.Empty);
            await sw.WriteLineAsync("// ReSharper disable all");
            await sw.WriteLineAsync("#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type");
            await sw.WriteLineAsync(string.Empty);
            await sw.WriteAsync(CommentHelper.Replace(await File.ReadAllTextAsync(generatedCsPath)));
            await sw.WriteLineAsync("#pragma warning restore");
        }

        _logger.Information("Done");
    }

    private async Task CopyResourcesAsync(params string[] namespaces)
    {
        foreach (var ns in namespaces)
        {
            Directory.CreateDirectory(Path.Combine(_options.PdfiumGeneratedProjectDir, ns));
        }

        var assm = GetType().Assembly;
        foreach (var fullName in assm.GetManifestResourceNames().Where(n => n.Contains("Resources")))
        {
            var parts = fullName.Split('.').Reverse().ToList();
            var file = string.Join('.', parts.Take(2).Reverse());
            var folder = parts.Skip(2).Take(1).ToList()[0];
            folder = "Resources".Equals(folder) || string.IsNullOrWhiteSpace(folder) ? string.Empty : folder;

            _logger.Information("Copying {File}", Path.GetFileName(fullName));

            var data = await BinaryData.FromStreamAsync(assm.GetManifestResourceStream(fullName)!);
            await File.WriteAllBytesAsync(Path.Combine(_options.PdfiumGeneratedProjectDir, folder, file), data.ToArray());
        }
    }


    private async Task DownloadAsync(LibraryInfo libraryInfo)
    {
        if (File.Exists(libraryInfo.FullFilePath))
        {
            _logger.Information("No need to download PDFium since the last time");
        }

        _logger.Information("Downloading {Filename}...", libraryInfo.Filename);
        await File.WriteAllBytesAsync(libraryInfo.FullFilePath, await _httpClient.GetByteArrayAsync(libraryInfo.BrowserDownloadUrl));
        _logger.Information("Download Complete");
    }

    private async Task ExtractAsync(LibraryInfo libraryInfo)
    {
        _logger.Information("Unzipping {Filename}...", libraryInfo.Filename);
        if (".zip".Equals(Path.GetExtension(libraryInfo.Filename), StringComparison.InvariantCultureIgnoreCase))
        {
            new FastZip().ExtractZip(libraryInfo.FullFilePath, libraryInfo.ExtractedLibBaseDirectory, null);
        }
        else
        {
            await using var fileStream = File.OpenRead(libraryInfo.FullFilePath);
            await using var inputStream = new GZipInputStream(fileStream);
            using var archive = TarArchive.CreateInputTarArchive(inputStream, Encoding.UTF8);
            archive.ExtractContents(libraryInfo.ExtractedLibBaseDirectory);
        }

        _logger.Information("Unzip complete");
    }

    [GeneratedRegex("'([^']*)")]
    private static partial Regex ValueRegex();
}