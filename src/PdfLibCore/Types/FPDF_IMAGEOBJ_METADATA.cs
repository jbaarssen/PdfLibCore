using System.Runtime.InteropServices;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
namespace PdfLibCore.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct FPDF_IMAGEOBJ_METADATA
    {
        // The image width in pixels.
        public uint Width { get; }

        // The image height in pixels.
        public uint Height { get; }

        // The image's horizontal pixel-per-inch.
        public float HorizontalDpi { get; }

        // The image's vertical pixel-per-inch.
        public float VerticalDpi { get; }

        // The number of bits used to represent each pixel.
        public uint BitsPerPixel { get; }

        // The image's colorspace. See above for the list of FPDF_COLORSPACE_*.
        public int Colorspace { get; }

        // The image's marked content ID. Useful for pairing with associated alt-text.
        // A value of -1 indicates no ID.
        public int MarkedContentId { get; }

        public FPDF_IMAGEOBJ_METADATA(uint width, uint height, float horizontalDpi, float verticalDpi, uint bitsPerPixel, int colorspace, int markedContentId)
        {
            Width = width;
            Height = height;
            HorizontalDpi = horizontalDpi;
            VerticalDpi = verticalDpi;
            BitsPerPixel = bitsPerPixel;
            Colorspace = colorspace;
            MarkedContentId = markedContentId;
        }
    }
}