using System.Collections.Generic;
using ParameterUsage = CppSharp.AST.ParameterUsage;

namespace PdfLibCore.CppSharp.Config;

public class Configuration
{
    public Dictionary<string, Class> Classes { get; set; }

    public Dictionary<string, Method> Methods { get; set; }
}

public class Class
{
    public Dictionary<string, Property> Properties { get; set; }
}

public class Property
{
    public bool Ignore { get; set; }
}

public class Method
{
    public string ReturnType { get; set; }
    public Dictionary<string, Parameter> Parameters { get; set; }
}

public class Parameter
{
    public ParameterUsage Usage { get; set; } = ParameterUsage.Unknown;
    public string Type { get; set; }
}