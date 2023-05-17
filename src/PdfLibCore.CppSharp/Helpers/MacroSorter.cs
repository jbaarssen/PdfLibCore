using System;
using System.Collections.Generic;
using System.Linq;
using CppAst;

namespace PdfLibCore.Parser.Helpers;

public class MacroList : List<Macro>
{
    public string Name { get; }

    public MacroList(string name)
    {
        Name = name;
    }
}

public class MacroSorter
{
    private MacroList? _macros;
    private readonly Dictionary<string, MacroSorter> _data = new();

    public MacroSorter(IEnumerable<CppMacro> macros)
    {
        foreach (var macro in macros)
        {
            Add(0, new Macro(macro));
        }
    }

    private MacroSorter()
    {
    }

    private MacroSorter Add(int index, Macro macro)
    {
        var key = macro.Parts[index];
        if (index <= Math.Min(macro.Count - 2, 1))
        {
            _data.TryAdd(key, new MacroSorter());
            return _data[key].Add(index + 1, macro);
        }
        (_macros ??= new MacroList(macro.GetPrefix(index))).Add(macro);
        return this;
    }

    public IEnumerable<MacroList> GetGroups()
    {
        foreach (var data in _data.SelectMany(x => x.Value.GetGroups()))
        {
            if (data.Count > 1)
            {
                yield return data;
            }
        }
        if ((_macros?.Any() ?? false) && _macros.Count > 1)
        {
            yield return _macros;
        }
    }
}