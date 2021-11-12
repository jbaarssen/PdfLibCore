using PdfLibCore.Types;

namespace PdfLibCore
{
    public class PdfPageRange : NativeWrapper<FPDF_PAGERANGE>
    {
        private readonly PdfDocument _document;

        public uint PrintPageRangeCount => Pdfium.FPDF_VIEWERREF_GetPrintPageRangeCount(Handle);

        public PdfPageRange(PdfDocument document, FPDF_PAGERANGE handle) 
            : base(handle)
        {
            _document = document;
        }
        
        public int PrintRangeElement(uint index) => Pdfium.FPDF_VIEWERREF_GetPrintPageRangeElement(Handle, index);

    }
}