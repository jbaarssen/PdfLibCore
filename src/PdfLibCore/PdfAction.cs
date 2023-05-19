using System;
using CppSharp.Runtime;
using PdfLibCore.Enums;
using PdfLibCore.Generated;
using PdfLibCore.Types;

namespace PdfLibCore;

public sealed class PdfAction : NativeDocumentWrapper<FPDF_Action>
{
    public ActionTypes Type => (ActionTypes) Pdfium.FPDFAction_GetType(Handle);

    public PdfDestination Destination => new(Document, Pdfium.FPDFAction_GetDest(Document.Handle, Handle), null);

    public string FilePath => MarshalUtil.GetString((ptr, len) => Pdfium.FPDFAction_GetFilePath(Handle, ptr, len)) ?? string.Empty;

    public Uri? Uri
    {
        get
        {
            var uri = MarshalUtil.GetString((ptr, len) => Pdfium.FPDFAction_GetURIPath(Document.Handle, Handle, ptr, len));
            return string.IsNullOrEmpty(uri) ? null : new Uri(uri);
        }
    }

    internal PdfAction(PdfDocument doc, FPDF_Action actionHandle)
        : base(doc, actionHandle)
    {
    }
}