using System;
using System.Linq;

namespace PdfLibCore.Parser.Helpers;

public static class NameHelper
{
    public static string GetName<T>()
        where T : Attribute
    {
        return typeof(T).Name.Replace(nameof(Attribute), string.Empty);
    }
    
    public static string ToCSharp(string name)
    {
        var toUpper = false;
        if (name.StartsWith("_"))
        {
            name = name[1..];
            toUpper = true;
        }
        if (name.EndsWith("_t__"))
        {
            name = name[..^4];
            toUpper = true;
        }
        if (name.EndsWith("_"))
        {
            name = name[..^1];
            toUpper = true;
        }
        return toUpper ? name.ToUpper() : name;
    }

    public static class Field
    {
        public static string ToCSharp(string name)
        {
            if (!name.Contains('_'))
            {
                return $"{char.ToUpper(name[0])}{name[1..]}";
            }
            var parts = name.Split('_');
            return string.Join(string.Empty, parts.Select(p => $"{char.ToUpper(p[0])}{p[1..]}"));
        }
    }
}