using System.Collections.Generic;
using PdfLibCore.Parser.Helpers;

namespace PdfLibCore.Parser.Converters;

public sealed partial class Converter
{
    private IEnumerable<CompiledSource> CreatePdfium()
    {
        yield return CreateIHandleInterface();
        foreach (var invoke in CreatePlatformInvoke())
        {
            yield return invoke;
        }
        yield return CreatePdfiumWrapper();
    }

    private CompiledSource CreatePdfiumWrapper()
    {
        var sourceElement = new SourceElement("Pdfium");
        var unit = GetCompilationUnit()
            .AddUsings(SyntaxHelper.AddUsingDirectives(
                "System",
                "System.Security",
                "System.Runtime.InteropServices",
                "PdfLibCore.Generated.Enums",
                "PdfLibCore.Generated.Structs",
                "PdfLibCore.Generated.Types"));

        return new CompiledSource(sourceElement, unit);
    }
}