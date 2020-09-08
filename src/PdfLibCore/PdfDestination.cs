using PdfLibCore.Types;

namespace PdfLibCore
{
    public sealed class PdfDestination : NativeWrapper<FPDF_DEST>
    {
        public PdfDocument Document { get; }
        public string Name { get; }

        public int PageIndex => Pdfium.FPDFDest_GetDestPageIndex(Document.Handle, Handle);

        public (float X, float Y, float Zoom) LocationInPage
        {
            get
            {
                if (!Pdfium.FPDFDest_GetLocationInPage(Handle, out bool hasX, out bool hasY, out bool hasZ, out float x, out float y, out float z))
                    return ((hasX ? x : float.NaN), (hasY ? y : float.NaN), (hasZ ? z : float.NaN));
                return (float.NaN, float.NaN, float.NaN);
            }
        }

        internal PdfDestination(PdfDocument doc, FPDF_DEST handle, string name)
            :base(handle)
        {
            Document = doc;
            Name = name;
        }
    }
}