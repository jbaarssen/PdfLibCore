using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PdfLibCore.Parser.Converters;
using Serilog;

namespace PdfLibCore.Parser;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static readonly string ProjectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName;
    private static readonly string GeneratedPath = Path.Combine(ProjectDir, "..", "PdfLibCore.Generated");
    private static readonly string HeaderPath = Path.Combine(ProjectDir, "Headers");

    private static readonly List<string> Files = new()
    {
        //"fpdf_annot.h",
        "fpdf_attachment.h",
        "fpdf_catalog.h",
        "fpdf_dataavail.h",
        "fpdf_doc.h",
        "fpdf_edit.h",
        "fpdf_ext.h",
        "fpdf_flatten.h",
        //"fpdf_formfill.h",
        //"fpdf_fwlevent.h",
        "fpdf_javascript.h",
        "fpdf_ppo.h",
        "fpdf_progressive.h",
        "fpdf_save.h",
        "fpdf_searchex.h",
        "fpdf_signature.h",
        "fpdf_structtree.h",
        "fpdf_sysfontinfo.h",
        "fpdf_text.h",
        "fpdf_thumbnail.h",
        "fpdf_transformpage.h",
        "fpdfview.h"
    };

    public static async Task Main(params string[] args)
    {
        Directory.CreateDirectory(HeaderPath);

        // Create logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(theme: ConsoleTheme.ColorScheme)
            .CreateLogger();

        // Clear generated
        Log.Information("Clearing old files");
        var generatedFiles = Directory.GetFiles(GeneratedPath, "*.cs", SearchOption.AllDirectories);
        foreach (var generated in generatedFiles)
        {
            File.Delete(generated);
        }
        Log.Information("Cleared {Count} old files", generatedFiles.Length);

        Log.Information("Getting header files");
        var result = await Task.WhenAll(DownloadFilesAsync());
        Log.Information("Got {Count} header files", result.Length);

        Log.Information("Saving header files");
        await Task.WhenAll(result.Select(SaveFileAsync));
        Log.Information("Saved {Count} header files", result.Length);

        Log.Information("Start C++ parsing");
        var cppConverter = new CppConverter(Log.Logger, result);
        Log.Information("Created C++ AST Context");

        if (cppConverter.Diagnostics.HasErrors)
        {
            foreach (var x in cppConverter.Diagnostics.Messages)
            {
                Log.Error("{Error}", x.Text);
            }
        }
        else
        {
            Log.Information("Start C++ conversion");
            foreach (var c in cppConverter.Convert())
            {
                Directory.CreateDirectory(string.IsNullOrEmpty(c.Directory) ? GeneratedPath : Path.Combine(GeneratedPath, c.Directory));
                await File.WriteAllTextAsync(Path.Combine(GeneratedPath, c.Path), c.ToString());
                Log.Information("Saved C# file '{File}'", c.Filename);
            }
            Log.Information("Saved {Count} C# files", Directory.GetFiles(GeneratedPath, "*.cs", SearchOption.AllDirectories).Length);
        }
    }

    private static IEnumerable<Task<Source>> DownloadFilesAsync()
    {
        var client = new HttpClient();
        foreach (var file in Files)
        {
            yield return Task.Run(async () =>
                new Source(Path.Combine(HeaderPath, file), await client.GetStringAsync(new Uri($"https://pdfium.googlesource.com/pdfium/+/master/public/{file}?format=TEXT"))));
        }
    }

    private static Task SaveFileAsync(Source source)
    {
        return Task.Run(async () =>
        {
            await File.WriteAllTextAsync(source.FullPath, source.DecodedContent);
            Log.Information("Saved '{File}'", source.Filename);
        });
    }
}