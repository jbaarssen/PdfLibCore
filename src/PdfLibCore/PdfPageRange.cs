using PdfLibCore.Types;

namespace PdfLibCore
{
    public class PdfPageRange : NativeWrapper<FPDF_PAGERANGE>
    {
        public uint PrintPageRangeCount => Pdfium.FPDF_VIEWERREF_GetPrintPageRangeCount(Handle);

        public PdfPageRange(PdfDocument document, FPDF_PAGERANGE handle) 
            : base(document, handle)
        {
        }
        
        public int PrintRangeElement(uint index) => Pdfium.FPDF_VIEWERREF_GetPrintPageRangeElement(Handle, index);

    }
}