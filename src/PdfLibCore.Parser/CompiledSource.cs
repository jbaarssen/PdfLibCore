using System;
using System.IO;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;

namespace PdfLibCore.Parser;

public sealed record CompiledSource(SourceElement Source, CompilationUnitSyntax Content)
{
    private const string FileExtension = ".g.cs";
    public string Filename => $"{NameHelper.ToCSharp(Source.Name)}{FileExtension}";
    public string? Directory => Source.Path;
    public string Path => string.IsNullOrWhiteSpace(Directory) ? Filename : System.IO.Path.Combine(Directory, Filename);

    public override string ToString()
    {
        return Content.NormalizeWhitespace().ToFullString();
    }
}

public sealed record Source(string FullPath, string Content)
{
    public string Filename => Path.GetFileName(FullPath);
    public string DecodedContent => Encoding.UTF8.GetString(Convert.FromBase64String(Content));
}

public sealed record SourceElement(string Name, CppElement? Element, string? Path = null)
{
    public SourceElement(string name, string? path = null)
        : this(name, null, path)
    {
    }
}