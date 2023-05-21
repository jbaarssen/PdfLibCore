using System.Collections.Generic;
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace PdfLibCore.CppSharp.Config;

public class Configuration
{
    public Dictionary<string, Class> Classes { get; private set; }

    public Dictionary<string, Method> Methods { get; private set; }
}