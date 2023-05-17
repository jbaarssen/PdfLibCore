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
            float Decide(FPDF_BOOL has, float value) => has ? value : float.NaN;

            return Pdfium.FPDFDest_GetLocationInPage(Handle, out var hasX, out var hasY, out var hasZ, out var x, out var y, out var z)
                ? new PdfPageLocation(Decide(hasX, x), Decide(hasY, y), Decide(hasZ, z))
                : PdfPageLocation.Unknown;
        }
    }

    internal PdfDestination(PdfDocument doc, FPDF_Dest handle, string? name)
        : base(doc, handle)
    {
        Name = name;
    }
}