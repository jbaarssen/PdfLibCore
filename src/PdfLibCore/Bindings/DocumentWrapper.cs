using System;
using System.Runtime.InteropServices;

namespace PdfLibCore.Bindings
{
    internal sealed class DocumentWrapper : IDisposable
    {
        private readonly IntPtr _handle;
        
        public FpdfDocumentT Instance { get; private set; }

        public DocumentWrapper(string filePath, string password)
        {
            Instance = fpdf_view.FPDF_LoadDocument(filePath, password);

            Conditions.NotNull(() => Instance);
        }
        
        public DocumentWrapper(byte[] bytes, string password)
        {
            _handle = Marshal.AllocHGlobal(bytes.Length);

            Marshal.Copy(bytes, 0, _handle, bytes.Length);

            Instance = fpdf_view.FPDF_LoadMemDocument(_handle, bytes.Length, password);

            Conditions.NotNull(() => Instance);
        }

        public DocumentWrapper(FpdfDocumentT instance)
        {
            Instance = instance;

            Conditions.NotNull(() => Instance);
        }

        public void Dispose()
        {
            Conditions.NotNull(() => Instance);
            
            fpdf_view.FPDF_CloseDocument(Instance);
            Marshal.FreeHGlobal(_handle);

            Instance = null;
        }
    }
}