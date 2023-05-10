using System;
using System.Collections.Generic;
using System.Linq;

namespace PdfLibCore.Parser;

public class Macro
{
    private readonly string _line;
    public List<string> Parts { get; }
    public int Count => Parts.Count;

    public Macro(string line)
    {
        _line = line;
        Parts = line.Split('_').ToList();
    }

    public override string ToString() => _line;

    public string GetPrefix(int index)
    {
        return string.Join('_', Parts.Take(index));
    }
}

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

    public void Add(Macro macro)
    {
        Add(0, macro);
    }

    private MacroSorter Add(int index, Macro macro)
    {
        var key = macro.Parts[index];
        if (index <= Math.Min(macro.Count - 2, 2))
        {
            _data.TryAdd(key, new MacroSorter());
            return _data[key].Add(index + 1, macro);
        }
        (_macros ??= new MacroList(macro.GetPrefix(index))).Add(macro);
        return this;
    }

    public IEnumerable<MacroList> GetGroups()
    {
        return _data.SelectMany(kvp => GetGroups(kvp.Value));
    }

    private static IEnumerable<MacroList> GetGroups(MacroSorter sorter)
    {
        foreach (var x in sorter._data.SelectMany(kvp => kvp.Value.GetGroups()))
        {
            yield return x;
        }
        if (sorter._macros?.Any() ?? false)
        {
            yield return sorter._macros;
        }
    }
}

public class Sorter
{
    public Sorter()
    {
        var macros = new List<Macro>
        {
            new("FPDF_UNSP_DOC_XFAFORM"),
            new("FPDF_UNSP_DOC_PORTABLECOLLECTION"),
            new("FPDF_UNSP_DOC_ATTACHMENT"),
            new("FPDF_UNSP_DOC_SECURITY"),
            new("FPDF_UNSP_DOC_SHAREDREVIEW"),
            new("FPDF_UNSP_DOC_SHAREDFORM_ACROBAT"),
            new("FPDF_UNSP_DOC_SHAREDFORM_FILESYSTEM"),
            new("FPDF_UNSP_DOC_SHAREDFORM_EMAIL"),
            new("FPDF_UNSP_ANNOT_3DANNOT"),
            new("FPDF_UNSP_ANNOT_MOVIE"),
            new("FPDF_UNSP_ANNOT_SOUND"),
            new("FPDF_UNSP_ANNOT_SCREEN_MEDIA"),
            new("FPDF_UNSP_ANNOT_SCREEN_RICHMEDIA"),
            new("FPDF_UNSP_ANNOT_ATTACHMENT"),
            new("FPDF_UNSP_ANNOT_SIG"),
            new("PAGEMODE_UNKNOWN"),
            new("PAGEMODE_USENONE"),
            new("PAGEMODE_USEOUTLINES"),
            new("PAGEMODE_USETHUMBS"),
            new("PAGEMODE_FULLSCREEN"),
            new("PAGEMODE_USEOC"),
            new("PAGEMODE_USEATTACHMENTS")
        };

        // Macros
        // FPDF_ANNOT_UNKNOWN - Group -> FPDF_ANNOT, Value -> UNKNOWN
        // FPDF_ANNOT_FLAG_NONE - Group -> FPDF_ANNOT_FLAG, Value -> NONE
        // FPDF_PRINTMODE_POSTSCRIPT3_TYPE42_PASSTHROUGH - Group -> FPDF_PRINTMODE, Value -> POSTSCRIPT3_TYPE42_PASSTHROUGH

        var x = new MacroSorter();

        foreach (var macro in macros)
        {
            Console.WriteLine($"Start {macro}");
            x.Add(macro);

            //Split(g);
            Console.WriteLine(macro);
            Console.WriteLine("----------------");
        }

        var groups = x.GetGroups();
    }

    private void Split(IGrouping<string, Macro> split)
    {
        Console.WriteLine($"Key: {split.Key} ({split.Count()})");
        foreach (var e in split)
        {
            Console.WriteLine($"Values: {string.Join(" --- ", e)}");
            //Split(e);
        }
    }

}