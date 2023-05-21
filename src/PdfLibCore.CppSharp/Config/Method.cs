using System.Collections.Generic;
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace PdfLibCore.CppSharp.Config;

public class Method
{
    public string ReturnType { get; private set; }
    public Dictionary<string, Parameter> Parameters { get; private set; }
}