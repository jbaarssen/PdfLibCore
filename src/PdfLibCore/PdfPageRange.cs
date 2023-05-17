using PdfLibCore.Generated;
using PdfLibCore.Types;

namespace PdfLibCore;

public class PdfPageRange : NativeDocumentWrapper<FPDF_Pagerange>
{
    public ulong PrintPageRangeCount => Pdfium.FPDF_VIEWERREF_GetPrintPageRangeCount(Handle);

    public PdfPageRange(PdfDocument document, FPDF_Pagerange handle)
        : base(document, handle)
    {
    }
        
    public int PrintRangeElement(uint index) => Pdfium.FPDF_VIEWERREF_GetPrintPageRangeElement(Handle, index);

}