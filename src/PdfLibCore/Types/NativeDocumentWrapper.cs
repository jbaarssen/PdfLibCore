using PdfLibCore.Generated;

namespace PdfLibCore.Types;

public abstract class NativeDocumentWrapper<T> : NativeWrapper<T>
    where T : class, ISafePointer
{
    protected PdfDocument Document { get; }

    protected NativeDocumentWrapper(PdfDocument document, T handle)
        : base(handle)
    {
        Document = document ?? throw new PdfiumException("Document is null");
    }
}