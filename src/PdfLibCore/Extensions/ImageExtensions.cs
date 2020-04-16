using System.Drawing.Imaging;
using System.IO;

// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image image, ImageFormat format = null)
        {
            using var ms = new MemoryStream();
            image.Save(ms, format ?? ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}