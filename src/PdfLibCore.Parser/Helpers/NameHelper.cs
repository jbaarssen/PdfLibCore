using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PdfLibCore.Parser.Helpers;

public static class NameHelper
{
    public static IdentifierNameSyntax GetName<T>()
        where T : class
    {
        return GetName(typeof(T));
    }

    public static IdentifierNameSyntax GetName<T>(T? item)
        where T : class
    {
        return SyntaxFactory.IdentifierName(item switch
        {
            SourceElement se => se.Name,
            Type t => t.Name.EndsWith(nameof(Attribute)) ? t.Name.Replace(nameof(Attribute), string.Empty) : t.Name,
            _ => ToCSharp(typeof(T).Name)
        });
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
        if (name.EndsWith("__"))
        {
            name = name[..^2];
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

            if (name.StartsWith("m_p", StringComparison.InvariantCultureIgnoreCase))
            {
                name = name[3..];
            }

            if (name.StartsWith("m_", StringComparison.InvariantCultureIgnoreCase))
            {
                name = name[2..];
            }

            var parts = name.Split('_');
            return string.Join(string.Empty, parts.Select(p => $"{char.ToUpper(p[0])}{p[1..]}"));
        }
    }
}