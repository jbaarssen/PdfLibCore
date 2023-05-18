using System.Runtime.InteropServices;

// ReSharper disable all
namespace PdfLibCore.Types;

[StructLayout(LayoutKind.Explicit)]
#pragma warning disable S101
public readonly struct FPDF_COLOR
#pragma warning restore
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
        ARGB = (uint)((a << 24) | (r << 16) | (g << 8) | b);
    }

    public FPDF_COLOR(uint argb)
        : this((byte) ((argb >> 16) & 0xff), (byte) ((argb >> 8) & 0xff), (byte) (argb & 0xff), (byte) ((argb >> 24) & 0xff))
    {
    }

    public static implicit operator FPDF_COLOR(uint argb) => new(argb);
    public static implicit operator uint(FPDF_COLOR color) => color.ARGB;
}