using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
namespace PdfLibCore.Types
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct FPDF_COLOR
    {
        [field: FieldOffset(0)]
        public byte A { get; }

        [field: FieldOffset(1)]
        public byte R { get; }

        [field: FieldOffset(2)]
        public byte G { get; }

        [field: FieldOffset(3)]
        public byte B { get; }

        [field: FieldOffset(0)]
        public uint ARGB { get; }

        public FPDF_COLOR(byte r, byte g, byte b, byte a = 255)
        {
            A = a;
            R = r;
            G = g;
            B = b;
            ARGB = a;
        }

        public FPDF_COLOR(uint argb)
        {
            A = 0;
            R = 0;
            G = 0; 
            B = 0;
            ARGB = argb;
        }

        public static implicit operator FPDF_COLOR(uint argb) => new(argb);
    }
}