// ReSharper disable once CheckNamespace
namespace PdfLibCore.Types;

public static class FpdfErrorExtension
{
    public static string GetDescription(this FPDF_ERR err) => err switch
    {
        FPDF_ERR.SUCCESS => "No error.",
        FPDF_ERR.UNKNOWN => "Unkown error.",
        FPDF_ERR.FILE => "File not found or could not be opened.",
        FPDF_ERR.FORMAT => "File not in PDF format or corrupted.",
        FPDF_ERR.PASSWORD => "Password required or incorrect password.",
        FPDF_ERR.SECURITY => "Unsupported security scheme.",
        FPDF_ERR.PAGE => "Page not found or content error.",
        FPDF_ERR.XFALOAD => "Load XFA error.",
        FPDF_ERR.XFALAYOUT => "Layout XFA error.",
        _ => $"{err} (No description available)."
    };
}