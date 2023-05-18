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
        : this($"PDFium Error: {Pdfium.FPDF_GetLastError()}")
    {
    }

    public PdfiumException(string message)
        : base(message)
    {
    }
}