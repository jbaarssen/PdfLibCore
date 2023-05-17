using System.Linq;
using CppSharp;
using CppSharp.Generators;
using CppSharp.Passes;
using PdfLibCore.CppSharp.Models;

namespace PdfLibCore.CppSharp.Passes;

public class RenameGeneratedClasses : GeneratorOutputPass
{
    private readonly RunnerOptions _options;

    public RenameGeneratedClasses(RunnerOptions options)
    {
        _options = options;
    }

    public override void VisitGeneratorOutput(GeneratorOutput output)
    {
        var filename = output.TranslationUnit.FileNameWithoutExtension;
        var texts = output.Outputs
            .SelectMany(i => i.FindBlocks(BlockKind.Unknown))
            .Where(b => b.Text.ToString().Contains(filename))
            .Select(b => b.Text);

        foreach (var text in texts)
        {
            text.StringBuilder.Replace(filename, _options.SharedLibraryName);
        }
    }
}