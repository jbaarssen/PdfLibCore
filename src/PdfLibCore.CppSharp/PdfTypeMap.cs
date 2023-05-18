using System;
using System.Collections.Generic;
using CppSharp.AST;
using CppSharp.Types;
using PdfLibCore.Types;
using Type = CppSharp.AST.Type;

namespace PdfLibCore.CppSharp;

public class PdfTypeMap : TypeMap
{
    private static readonly Dictionary<string, Func<System.Type>> Types = new()
    {
        { nameof(FPDF_ERR), () => typeof(FPDF_ERR) },
        { nameof(FPDF_BOOL), () => typeof(FPDF_BOOL) },
        { nameof(FPDF_COLOR), () => typeof(FPDF_COLOR) },
        { "FPDF_WIDESTRING", () => typeof(string) },
        { "FPDF_WCHAR", () => typeof(string) },
        { "FPDF_BYTESTRING", () => typeof(string) },
        { "FPDF_STRING", () => typeof(string) },
        { "FPDF_DWORD", () => typeof(long) },
        { "FPDF_RESULT", () => typeof(int) }
    };

    public static void Register(TypeMapDatabase typeMap)
    {
        foreach (var type in Types.Keys)
        {
            typeMap.TypeMaps.TryAdd(type, new PdfTypeMap(type));
        }
    }

    private readonly string _name;
    private readonly CILType _type;

    private PdfTypeMap(string name)
    {
        _name = name;
        _type = new CILType(Types[_name]());
        Type = _type;
    }

    public override Type CSharpSignatureType(TypePrinterContext ctx) => _type;

    public override string ToString() => _name;
}