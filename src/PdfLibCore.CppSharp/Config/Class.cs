using System.Collections.Generic;

// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace PdfLibCore.CppSharp.Config;

public class Class
{
    public Dictionary<string, Property> Properties { get; private set; }
}