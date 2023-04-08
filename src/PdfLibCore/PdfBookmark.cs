using System.Collections.Generic;
using PdfLibCore.Types;

namespace PdfLibCore
{
    public sealed class PdfBookmark : NativeWrapper<FPDF_BOOKMARK>
    {
        public IEnumerable<PdfBookmark> Children
        {
            get
            {
                var handle = Pdfium.FPDFBookmark_GetFirstChild(Document.Handle, Handle);
                while (!handle.IsNull)
                {
                    yield return new PdfBookmark(Document, handle);
                    handle = Pdfium.FPDFBookmark_GetNextSibling(Document.Handle, handle);
                }
            }
        }

        public string Title => Pdfium.FPDFBookmark_GetTitle(Handle);

        public PdfDestination Destination => new(Document, Pdfium.FPDFBookmark_GetDest(Document.Handle, Handle), null);

        public PdfAction Action => new(Document, Pdfium.FPDFBookmark_GetAction(Handle));

        internal PdfBookmark(PdfDocument doc, FPDF_BOOKMARK handle)
            : base(doc, handle)
        {
        }
    }
}