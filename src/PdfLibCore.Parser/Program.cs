using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PdfLibCore.Parser.Converters;

namespace PdfLibCore.Parser;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static readonly string ProjectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName;
    private static readonly string HeaderPath = Path.Combine(ProjectDir, "Headers");

    public static async Task Main(params string[] args)
    {
         // Clear generated
        var generatedFiles = Directory.GetFiles(Path.Combine(ProjectDir, "Generated"));
        foreach (var generated in generatedFiles)
        {
            File.Delete(generated);
        }

        var files = new List<string>();
        await foreach (var file in GetFiles(false))
        {
            files.Add(file);
        }

        var cppConverter = new CppConverter(files);

        if (cppConverter.Diagnostics.HasErrors)
        {
            foreach (var x in cppConverter.Diagnostics.Messages)
            {
                Console.WriteLine(x.Text);
            }
        }
        else
        {
            var converted = cppConverter.Convert();
            foreach (var c in converted)
            {
                await File.WriteAllTextAsync(Path.Combine(ProjectDir, "Generated", $"{c.Filename}"), c.ToString());
            }
        }
    }

    private static async IAsyncEnumerable<string> GetFiles(bool download = true)
    {
        var client = new HttpClient();
        yield return await SaveFile(client, "fpdfview.h", download);
        yield return await SaveFile(client, "fpdf_doc.h", download);
        yield return await SaveFile(client, "fpdf_edit.h", download);
        yield return await SaveFile(client, "fpdf_ext.h", download);
        yield return await SaveFile(client, "fpdf_flatten.h", download);
        yield return await SaveFile(client, "fpdf_ppo.h", download);
        yield return await SaveFile(client, "fpdf_progressive.h", download);
        yield return await SaveFile(client, "fpdf_save.h", download);
        yield return await SaveFile(client, "fpdf_searchex.h", download);
        yield return await SaveFile(client, "fpdf_structtree.h", download);
        yield return await SaveFile(client, "fpdf_text.h", download);
        yield return await SaveFile(client, "fpdf_transformpage.h", download);
    }

    private static async Task<string> SaveFile(HttpClient client, string file, bool download)
    {
        if (!download)
        {
            return Path.Combine(HeaderPath, file);
        }

        Directory.CreateDirectory(HeaderPath);
        var result = await client.GetStringAsync(new Uri($"https://pdfium.googlesource.com/pdfium/+/master/public/{file}?format=TEXT"));
        await File.WriteAllTextAsync(Path.Combine(HeaderPath, file), Encoding.UTF8.GetString(Convert.FromBase64String(result)));
        return Path.Combine(HeaderPath, file);
    }
}