using PdfLibCore.Generated;
using PdfLibCore.Types;

namespace PdfLibCore;

public sealed class PdfDestination : NativeDocumentWrapper<FPDF_Dest>
{
    public string? Name { get; }

    public int PageIndex => Pdfium.FPDFDest_GetDestPageIndex(Document.Handle, Handle);

    public PdfPageLocation LocationInPage
    {
        get
        {
            var hasX = FPDF_BOOL.False;
            var hasY = FPDF_BOOL.False;
            var hasZ = FPDF_BOOL.False;
            var x = 0f;
            var y = 0f;
            var z = 0f;
            return Pdfium.FPDFDest_GetLocationInPage(Handle, ref hasX, ref hasY, ref hasZ, ref x, ref y, ref z)
                ? new PdfPageLocation(hasX ? x : float.NaN, hasY ? y : float.NaN, hasZ ? z : float.NaN)
                : PdfPageLocation.Unknown;
        }
    }

    internal PdfDestination(PdfDocument doc, FPDF_Dest handle, string? name)
        : base(doc, handle)
    {
        Name = name;
    }
}