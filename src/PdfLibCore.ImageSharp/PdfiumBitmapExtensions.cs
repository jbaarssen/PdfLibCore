using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;

// ReSharper disable once CheckNamespace
namespace PdfLibCore.ImageSharp
{
    public static class PdfiumBitmapExtensions
    {
        public static Image AsImage(this PdfiumBitmap bitmap, double dpiX = 72, double dpiY = 72) => 
            BmpDecoder.Instance.Decode(new BmpDecoderOptions(), bitmap.AsBmpStream(dpiX, dpiY));
    }
}