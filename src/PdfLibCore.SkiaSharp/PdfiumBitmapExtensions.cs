using SkiaSharp;

// ReSharper disable once CheckNamespace
namespace PdfLibCore.SkiaSharp
{
    public static class PdfiumBitmapExtensions
    {
        public static SKBitmap AsImage(this PdfiumBitmap bitmap, double dpiX = 72, double dpiY = 72) => 
            SKBitmap.Decode(bitmap.AsBmpStream(dpiX, dpiY));
    }
}