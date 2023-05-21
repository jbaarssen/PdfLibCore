using CppSharp.AST;
// ReSharper disable ClassNeverInstantiated.Global

namespace PdfLibCore.CppSharp.Config;

public class Parameter
{
    public ParameterUsage Usage { get; private set; } = ParameterUsage.Unknown;
    public string Type { get; private set; }
}