using ImageMagick;

// ReSharper disable once CheckNamespace
namespace PdfLibCore.MagickNet
{
    public static class PdfiumBitmapExtensions
    {
        public static MagickImage AsImage(this PdfiumBitmap bitmap, double dpiX = 72, double dpiY = 72) => 
            new MagickImage(bitmap.AsBmpStream(dpiX, dpiY));
    }
}