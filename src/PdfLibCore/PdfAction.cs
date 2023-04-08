using System;
using PdfLibCore.Enums;
using PdfLibCore.Types;

namespace PdfLibCore
{
    public sealed class PdfAction : NativeWrapper<FPDF_ACTION>
    {
        public ActionTypes Type => Pdfium.FPDFAction_GetType(Handle);

        public PdfDestination Destination => new(Document, Pdfium.FPDFAction_GetDest(Document.Handle, Handle), null);

        public string FilePath => Pdfium.FPDFAction_GetFilePath(Handle);

        public Uri Uri => new(Pdfium.FPDFAction_GetURIPath(Document.Handle, Handle));

        internal PdfAction(PdfDocument doc, FPDF_ACTION actionHandle)
            : base(doc, actionHandle)
        {
        }
    }
}