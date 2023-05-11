using System;
using System.Collections.Generic;
using System.Linq;
using CppAst;
using PdfLibCore.Parser.Helpers;

namespace PdfLibCore.Parser;

public class SorterTest
{
    public SorterTest()
    {
        var macros = new List<CppMacro>
        {   new("FPDF_ERR_SUCCESS"),
            new("FPDF_ERR_UNKNOWN"),
            new("FPDF_ERR_FILE"),
            new("FPDF_ERR_FORMAT"),
            new("FPDF_ERR_PASSWORD"),
            new("FPDF_ERR_SECURITY"),
            new("FPDF_ERR_PAGE"),
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
            new("PAGEMODE_USEATTACHMENTS"),

        };

        // Macros
        // FPDF_ANNOT_UNKNOWN - Group -> FPDF_ANNOT, Value -> UNKNOWN
        // FPDF_ANNOT_FLAG_NONE - Group -> FPDF_ANNOT_FLAG, Value -> NONE
        // FPDF_PRINTMODE_POSTSCRIPT3_TYPE42_PASSTHROUGH - Group -> FPDF_PRINTMODE, Value -> POSTSCRIPT3_TYPE42_PASSTHROUGH

        var x = new MacroSorter(macros);

        var groups = x.GetGroups();
        foreach (var group in groups)
        {
            foreach (var item in group)
            {
                Console.WriteLine($"{item}");
            }
            Console.WriteLine("----------------");
        }
    }
}
