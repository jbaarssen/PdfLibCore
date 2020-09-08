using System;
using PdfLibCore.Types;

namespace PdfLibCore
{
    public sealed class PdfiumException : Exception
    {
        public PdfiumException()
            : base($"PDFium Error: {Pdfium.FPDF_GetLastError().GetDescription()}") { }
    }
}