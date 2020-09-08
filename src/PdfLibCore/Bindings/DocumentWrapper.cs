using System;
using System.Runtime.InteropServices;

namespace PdfLibCore.Bindings
{
    internal sealed class DocumentWrapper : IDisposable
    {
        public FpdfDocumentT Instance { get; }

        public DocumentWrapper(byte[] bytes, string password)
        {
            Instance = fpdf_view.FPDF_LoadDocument(bytes, 0, -1, password);
        }

        public DocumentWrapper(FpdfDocumentT instance)
        {
            Instance = instance;
        }

        public void Dispose()
        {
            Instance.SetToNull();
        }
    }
}