using System.Linq;
using CppSharp;
using CppSharp.Generators;
using CppSharp.Passes;
using PdfLibCore.Generated;

namespace PdfLibCore.CppSharp.Passes;

public class AddSafePointerInterface : GeneratorOutputPass
{
    public override void VisitGeneratorOutput(GeneratorOutput output)
    {
        base.VisitGeneratorOutput(output);

        var names = output.TranslationUnit.Classes
            .Where(c => c.IsIncomplete)
            .Select(c => c.Name)
            .ToList();
        var classes = output.Outputs
            .SelectMany(i => i.FindBlocks(BlockKind.Class)
                .SelectMany(b => b.Blocks)
                .Select(b => b.Text))
            .ToList();

        foreach (var name in names)
        {
            var found = classes.FirstOrDefault(x => x.ToString().Contains(name));
            found?.StringBuilder.Replace(name, $"{name} : {nameof(ISafePointer)}");
        }
    }
}