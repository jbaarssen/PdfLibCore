using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PdfLibCore.Parser.Converters;

public interface ICppConverter
{
    CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit);
}