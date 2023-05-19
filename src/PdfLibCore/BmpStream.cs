using System;
using System.IO;
using System.Runtime.InteropServices;
using PdfLibCore.Enums;

namespace PdfLibCore;

internal sealed class BmpStream : Stream
{
    private const double MetersPerInch = 0.0254;
    private const uint BmpHeaderSize = 14;
    private const uint DibHeaderSize = 108; // BITMAPV4HEADER
    private const uint PixelArrayOffset = BmpHeaderSize + DibHeaderSize;
    private const uint CompressionMethod = 3; // BI_BITFIELDS
    private const uint MaskR = 0x00_FF_00_00;
    private const uint MaskG = 0x00_00_FF_00;
    private const uint MaskB = 0x00_00_00_FF;
    private const uint MaskA = 0xFF_00_00_00;

    private readonly PdfiumBitmap _bitmap;
    private readonly uint _length;
    private readonly uint _stride;
    private readonly uint _rowLength;
    private uint _pos;
    private readonly double _dpiX;
    private readonly double _dpiY;

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override long Length => _length;

    public override long Position
    {
        get => _pos;
        set => _pos = (uint) value;
    }

    public BmpStream(PdfiumBitmap bitmap, double dpiX, double dpiY)
    {
        _dpiX = dpiX;
        _dpiY = dpiY;
        _bitmap = bitmap.Format == BitmapFormats.Gray
            ? throw new NotSupportedException($"Bitmap format {bitmap.Format} is not yet supported.")
            : bitmap;
        _rowLength = (uint) bitmap.BytesPerPixel * (uint) bitmap.Width;
        _stride = ((uint) bitmap.BytesPerPixel * 8 * (uint) bitmap.Width + 31) / 32 * 4;
        _length = PixelArrayOffset + _stride * (uint) bitmap.Height;
        _pos = 0;
    }

    private static byte[] GetHeader(uint fileSize, PdfiumBitmap bitmap, double dpiX, double dpiY)
    {
        var header = new byte[BmpHeaderSize + DibHeaderSize];

        using var ms = new MemoryStream(header);
        using var writer = new BinaryWriter(ms);
        writer.Write((byte) 'B');
        writer.Write((byte) 'M');
        writer.Write(fileSize);
        writer.Write(0u);
        writer.Write(PixelArrayOffset);
        writer.Write(DibHeaderSize);
        writer.Write(bitmap.Width);
        writer.Write(-bitmap.Height); // top-down image
        writer.Write((ushort) 1);
        writer.Write((ushort) (bitmap.BytesPerPixel * 8));
        writer.Write(CompressionMethod);
        writer.Write(0);
        writer.Write((int) Math.Round(dpiX / MetersPerInch));
        writer.Write((int) Math.Round(dpiY / MetersPerInch));
        writer.Write(0L);
        writer.Write(MaskR);
        writer.Write(MaskG);
        writer.Write(MaskB);
        if (bitmap.Format == BitmapFormats.RGBA)
        {
            writer.Write(MaskA);
        }
        return header;
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var bytesToRead = count;
        var returnValue = 0;
        if (_pos < PixelArrayOffset)
        {
            returnValue = Math.Min(count, (int) (PixelArrayOffset - _pos));
            Buffer.BlockCopy(
                GetHeader(_length, _bitmap, _dpiX, _dpiY),
                (int) _pos,
                buffer,
                offset,
                returnValue);
            _pos += (uint) returnValue;
            offset += returnValue;
            bytesToRead -= returnValue;
        }

        if (bytesToRead <= 0)
        {
            return returnValue;
        }

        bytesToRead = Math.Min(bytesToRead, (int) (_length - _pos));
        var idxBuffer = _pos - PixelArrayOffset;

        if (_stride == _bitmap.Stride)
        {
            Marshal.Copy(_bitmap.Scan0 + (int) idxBuffer, buffer, offset, bytesToRead);
            returnValue += bytesToRead;
            _pos += (uint) bytesToRead;
            return returnValue;
        }

        void Handle(int read)
        {
            offset += read;
            idxBuffer += (uint) read;
            bytesToRead -= read;
            returnValue += read;
        }

        while (bytesToRead > 0)
        {
            var read = Math.Min(
                bytesToRead,
                Math.Max(0, (int) _rowLength - (int) (idxBuffer / _stride)));
            if (read > 0)
            {
                Marshal.Copy(_bitmap.Scan0 + (int) idxBuffer, buffer, offset, read);
            }
            Handle(read);
            read = Math.Min(bytesToRead, (int) (_stride - _rowLength));
            for (var i = 0; i < read; i++)
            {
                buffer[offset + i] = 0;
            }
            Handle(read);
        }
        _pos = PixelArrayOffset + idxBuffer;
        return returnValue;
    }

    public override long Seek(long offset, SeekOrigin origin) => Position = origin switch
    {
        SeekOrigin.Begin => offset,
        SeekOrigin.Current => Position + offset,
        SeekOrigin.End => Length + offset,
        _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
    };

    public override void SetLength(long value) =>
        throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException();
}