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
            FPDF_BOOL hasX = 0;
            FPDF_BOOL hasY = 0;
            FPDF_BOOL hasZ = 0;
            float x = 0;
            float y = 0;
            float z = 0;
            var x2 =  Pdfium.FPDFDest_GetLocationInPage(Handle, ref hasX, ref hasY, ref hasZ, ref x, ref y, ref z)
                ? new PdfPageLocation(hasX ? x : float.NaN, hasY ? y : float.NaN, hasZ ? z : float.NaN)
                : PdfPageLocation.Unknown;
            return x2;
        }
    }

    internal PdfDestination(PdfDocument doc, FPDF_Dest handle, string? name)
        : base(doc, handle)
    {
        Name = name;
    }
}