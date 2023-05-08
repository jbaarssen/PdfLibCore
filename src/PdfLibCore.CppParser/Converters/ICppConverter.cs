using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PdfLibCore.CppParser.Converters;

public interface ICppConverter
{
    CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit);
}