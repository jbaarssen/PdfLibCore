namespace PdfLibCore.Types
{
    public enum FPDF_ERR : uint
    {
        /// <summary>
        /// No error.
        /// </summary>
        SUCCESS = 0,

        /// <summary>
        /// Unknown error.
        /// </summary>
        UNKNOWN = 1,

        /// <summary>
        /// File not found or could not be opened.
        /// </summary>
        FILE = 2,

        /// <summary>
        /// File not in PDF format or corrupted.
        /// </summary>
        FORMAT = 3,

        /// <summary>
        /// Password required or incorrect password.
        /// </summary>
        PASSWORD = 4,

        /// <summary>
        /// Unsupported security scheme.
        /// </summary>
        SECURITY = 5,

        /// <summary>
        /// Page not found or content error.
        /// </summary>
        PAGE = 6,

        /// <summary>
        /// Load XFA error.
        /// </summary>
        XFALOAD = 7,

        /// <summary>
        /// Layout XFA error.
        /// </summary>
        XFALAYOUT = 8
    }

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
}