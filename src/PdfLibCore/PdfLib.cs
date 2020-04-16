using PdfLibCore.Bindings;
using PdfLibCore.Readers;

namespace PdfLibCore
{
    public sealed class PdfLib : IPdfLib
    {
        /// <summary>
        /// PDFium is not thread-safe
        /// so we need to lock every native
        /// call. We might implement
        /// Command patter or something similar
        /// to get around this in the future.
        /// </summary>
        internal static readonly object Lock = new object();

        private static PdfLib _instance;

        private PdfLib()
        {
            fpdf_view.FPDF_InitLibrary();
        }

        public static PdfLib Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PdfLib();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Dispose()
        {
            lock (Lock)
            {
                fpdf_view.FPDF_DestroyLibrary();
            }

            _instance = null;
        }

        /// <inheritdoc />
        public IDocumentReader GetDocumentReader(string filePath)
        {
            return GetDocumentReader(filePath, null);
        }

        /// <inheritdoc />
        public IDocumentReader GetDocumentReader(string filePath, string password)
        {
            Conditions.NotNull(() => filePath);

            return new DocumentReader(filePath, password);
        }

        /// <inheritdoc />
        public IDocumentReader GetDocumentReader(byte[] bytes)
        {
            return GetDocumentReader(bytes, null);
        }

        /// <inheritdoc />
        public IDocumentReader GetDocumentReader(byte[] bytes, string password)
        {
            Conditions.NotNull(() => bytes);

            return new DocumentReader(bytes, password);
        }

        public string GetLastError()
        {
            lock (Lock)
            {
                var code = fpdf_view.FPDF_GetLastError();

                switch (code)
                {
                    case 0:
                        return "no error";
                    case 1:
                        return "unknown error";
                    case 2:
                        return "file not found or could not be opened";
                    case 3:
                        return "file not in PDF format or corrupted";
                    case 4:
                        return "password required or incorrect password";
                    case 5:
                        return "unsupported security scheme";
                    case 6:
                        return "page not found or content error";
                    case 1001:
                        return "the requested operation cannot be completed due to a license restrictions";
                    default:
                        return "unknown error";
                }
            }
        }
    }
}