// ReSharper disable InconsistentNaming
namespace PdfLibCore.Types
{
    /// <summary>
    /// PDF text rendering modes
    /// </summary>
    public enum FPDF_TEXT_RENDERMODE
    {
        UNKNOWN = -1,
        FILL = 0,
        STROKE = 1,
        FILL_STROKE = 2,
        INVISIBLE = 3,
        FILL_CLIP = 4,
        STROKE_CLIP = 5,
        FILL_STROKE_CLIP = 6,
        CLIP = 7,
        LAST = CLIP,
    }
}