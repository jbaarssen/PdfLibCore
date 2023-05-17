using System;
using PdfLibCore.Generated;

namespace PdfLibCore;

public sealed class PdfiumException : Exception
{
    public PdfiumException(Exception innerException)
        : base(innerException.Message, innerException)
    {
    }

    public PdfiumException()
        : base($"PDFium Error: {Pdfium.FPDF_GetLastError()}")
    {
    }
}