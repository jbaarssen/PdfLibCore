using System;
using PdfLibCore.Types;

namespace PdfLibCore
{
    public sealed class PdfiumException : Exception
    {
        public PdfiumException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public PdfiumException()
            : base($"PDFium Error: {Pdfium.FPDF_GetLastError().GetDescription()}")
        {
        }
    }
}