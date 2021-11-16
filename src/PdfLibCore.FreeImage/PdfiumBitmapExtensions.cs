using FreeImageAPI;

// ReSharper disable once CheckNamespace
namespace PdfLibCore.FreeImageAPI
{
    public static class PdfiumBitmapExtensions
    {
        public static FIBITMAP AsImage(this PdfiumBitmap bitmap, double dpiX = 72, double dpiY = 72) => 
            FreeImage.LoadFromStream(bitmap.AsBmpStream(dpiX, dpiY));
    }
}