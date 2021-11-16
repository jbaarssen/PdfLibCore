using PdfLibCore.Types;

namespace PdfLibCore
{
    public sealed class PdfDestination : NativeWrapper<FPDF_DEST>
    {
        public string Name { get; }

        public int PageIndex => Pdfium.FPDFDest_GetDestPageIndex(Document.Handle, Handle);

        public PdfPageLocation LocationInPage => 
            Pdfium.FPDFDest_GetLocationInPage(Handle, out var hasX, out var hasY, out var hasZ, out var x, out var y, out var z) 
                ? PdfPageLocation.Unknown 
                : new PdfPageLocation(hasX ? x : float.NaN, hasY ? y : float.NaN, hasZ ? z : float.NaN);

        internal PdfDestination(PdfDocument doc, FPDF_DEST handle, string name)
            : base(doc, handle)
        {
            Name = name;
        }
    }
}