using System.Collections.Generic;
using PdfLibCore.Generated;
using PdfLibCore.Helpers;
using PdfLibCore.Types;

namespace PdfLibCore;

public sealed class PdfBookmark : NativeDocumentWrapper<FPDF_Bookmark>
{
    public string Title => Helper.GetString((ptr, len) => Pdfium.FPDFBookmark_GetTitle(Handle, ptr, len)) ?? string.Empty;

    public PdfDestination Destination => new(Document, Pdfium.FPDFBookmark_GetDest(Document.Handle, Handle), null);

    public IEnumerable<PdfBookmark> Children
    {
        get
        {
            var handle = Pdfium.FPDFBookmark_GetFirstChild(Document.Handle, Handle);
            while (!handle.IsNull())
            {
                yield return new PdfBookmark(Document, handle);
                handle = Pdfium.FPDFBookmark_GetNextSibling(Document.Handle, handle);
            }
        }
    }

    public PdfAction? Action
    {
        get
        {
            var handle = Pdfium.FPDFBookmark_GetAction(Handle);
            return handle != null ? new PdfAction(Document, handle) : null;
        }
    }

    internal PdfBookmark(PdfDocument doc, FPDF_Bookmark handle)
        : base(doc, handle)
    {
    }

    public static PdfBookmark? Create(PdfDocument doc, FPDF_Bookmark handle) => handle.IsNull() ? null : new PdfBookmark(doc, handle);
}