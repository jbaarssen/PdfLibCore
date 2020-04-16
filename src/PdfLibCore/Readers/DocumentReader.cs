using System;
using PdfLibCore.Bindings;
using PdfLibCore.Models;

namespace PdfLibCore.Readers
{
    public class DocumentReader : IDocumentReader
    {
        private readonly DocumentWrapper _documentWrapper;
        
        public DocumentReader(string filePath, string password)
        {
            lock (PdfLib.Lock)
            {
                _documentWrapper = new DocumentWrapper(filePath, password);
            }
        }
        
        public DocumentReader(byte[] bytes, string password)
        {
            lock (PdfLib.Lock)
            {
                _documentWrapper = new DocumentWrapper(bytes, password);
            }
        }
        
        public void Dispose()
        {
            lock (PdfLib.Lock)
            {
                _documentWrapper?.Dispose();
            }
        }

        public PdfVersion GetPdfVersion()
        {
            var version = 0;

            lock (PdfLib.Lock)
            {
                var success = fpdf_view.FPDF_GetFileVersion(_documentWrapper.Instance, ref version) == 1;

                if (!success)
                {
                    throw new Exception("failed to get pdf version");
                }
            }

            return new PdfVersion(version);
        }

        public int GetPageCount()
        {
            lock (PdfLib.Lock)
            {
                return fpdf_view.FPDF_GetPageCount(_documentWrapper.Instance);
            }
        }

        public IPageReader GetPageReader(int pageIndex)
        {
            return new PageReader(_documentWrapper, pageIndex);
        }
    }
}