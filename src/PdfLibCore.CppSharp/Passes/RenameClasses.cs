using System.Text.RegularExpressions;
using CppSharp.AST;
using CppSharp.Passes;

namespace PdfLibCore.CppSharp.Passes;

public partial class RenameClasses : TranslationUnitPass
{
    public override bool VisitClassDecl(Class @class)
    {
        var match = ClassNameRegex().Match(@class.Name);
        if (match.Success)
        {
            @class.Name = Rename(match.Groups[1].Value);
        }

        return base.VisitClassDecl(@class);
    }

    private static string Rename(string className) => $"FPDF_{char.ToUpper(className[0])}{className[1..]}";

    [GeneratedRegex("fpdf_(.*)_t__")]
    private static partial Regex ClassNameRegex();
}