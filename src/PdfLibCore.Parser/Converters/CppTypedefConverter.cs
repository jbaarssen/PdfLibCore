using System;
using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PdfLibCore.Parser.Converters;

public sealed class CppTypedefConverter : CppClassConverter
{
    private static CppClass DetermineElement(CppTypedef cppElement)
    {
        if (cppElement.Parent is not CppCompilation c)
        {
            throw new Exception(cppElement.Name);
        }
        if (cppElement.ElementType is not CppPointerType { ElementType: CppClass pointerClass })
        {
            throw new Exception(cppElement.Name);
        }

        var found = c.Classes.FirstOrDefault(c => c.Name == pointerClass.Name);
        if (found == null)
        {
            throw new Exception(cppElement.Name);
        }
        return found;
    }

    public CppTypedefConverter(CppTypedef cppElement)
        : base(cppElement.Name, DetermineElement(cppElement))
    {
    }
}