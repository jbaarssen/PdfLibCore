using System.Collections.Generic;
using System.Linq;
using CppAst;

namespace PdfLibCore.Parser.Helpers;

public class Macro
{
    public CppMacro Value { get; }
    public List<string> Parts { get; }
    public int Count => Parts.Count;

    public Macro(CppMacro macro)
    {
        Value = macro;
        Parts = Value.Name.Split('_').ToList();
    }

    public override string ToString() => Value.ToString();

    public string GetPrefix(int index)
    {
        return string.Join('_', Parts.Take(index));
    }
}