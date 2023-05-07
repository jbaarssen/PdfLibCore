using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;

namespace PdfLibCore.Parser;

public sealed record CompiledSource(string Filename, CompilationUnitSyntax Content)
{
    private const string FileExtension = ".g.cs";
    private readonly string _filename = NameHelper.ToCSharp(Filename);
    public string Filename => $"{_filename}{FileExtension}";

    public override string ToString()
    {
        return Content.NormalizeWhitespace().ToFullString();
    }
}