using System;
using System.IO;
using System.Runtime.InteropServices;
using PdfLibCore.Enums;
using PdfLibCore.Types;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;

namespace PdfLibCore
{
    /// <summary>
	/// A bitmap to which a <see cref="PdfPage"/> can be rendered.
	/// </summary>
    public sealed class PdfiumBitmap : NativeWrapper<FPDF_BITMAP>
    {
	    private readonly bool _forceAlphaChannel;

	    public int Width => Pdfium.FPDFBitmap_GetWidth(Handle);
		public int Height => Pdfium.FPDFBitmap_GetHeight(Handle);
		public int Stride => Pdfium.FPDFBitmap_GetStride(Handle);
		public IntPtr Scan0 => Pdfium.FPDFBitmap_GetBuffer(Handle);
		public int BytesPerPixel => GetBytesPerPixel(Format);
		public BitmapFormats Format { get; }

		private PdfiumBitmap(FPDF_BITMAP bitmap, BitmapFormats format)
			: base(bitmap.IsNull ? throw new Exception() : bitmap)
		{
			GetBytesPerPixel(format);
			Format = format;
		}
		
		~PdfiumBitmap()
		{
		}

		/// <summary>
		/// Creates a new <see cref="PdfiumBitmap"/>. Unmanaged memory is allocated which must
		/// be freed by calling <see cref="Dispose"/>.
		/// </summary>
		/// <param name="width">The width of the new bitmap.</param>
		/// <param name="height">The height of the new bitmap.</param>
		/// <param name="hasAlpha">A value indicating wheter the new bitmap has an alpha channel.</param>
		/// <param name="forceAlphaChannel"></param>
		/// <remarks>
		/// A bitmap created with this overload always uses 4 bytes per pixel.
		/// Depending on <paramref name="hasAlpha"/> the <see cref="Format"/> is then either
		/// <see cref="BitmapFormats.RGBA"/> or <see cref="BitmapFormats.RGBx"/>.
		/// </remarks>
		public PdfiumBitmap(int width, int height, bool hasAlpha, bool forceAlphaChannel = false)
			: this(Pdfium.FPDFBitmap_Create(width, height, hasAlpha), hasAlpha ? BitmapFormats.RGBA : BitmapFormats.RGBx)
		{
			_forceAlphaChannel = forceAlphaChannel;
			FillRectangle(0, 0, width, height, 0xFFFFFFFF);
		}

		/// <summary>
		/// Creates a new <see cref="PdfiumBitmap"/> using memory allocated by the caller.
		/// The caller is responsible for freeing the memory and that the adress stays
		/// valid during the lifetime of the returned <see cref="PdfiumBitmap"/>. To free
		/// unmanaged resources, <see cref="Dispose"/> must be called.
		/// </summary>
		/// <param name="width">The width of the new bitmap.</param>
		/// <param name="height">The height of the new bitmap.</param>
		/// <param name="format">The format of the new bitmap.</param>
		/// <param name="scan0">The adress of the memory block which holds the pixel values.</param>
		/// <param name="stride">The number of bytes per image row.</param>
		public PdfiumBitmap(int width, int height, BitmapFormats format, IntPtr scan0, int stride)
			: this(Pdfium.FPDFBitmap_CreateEx(width, height, format, scan0, stride), format)
		{
			FillRectangle(0, 0, width, height, 0xFFFFFFFF);
		}

		public Image AsImage(double dpiX = 72, double dpiY = 72)
		{
			var image = AsBmpStream(dpiX, dpiY);
			if (image == null)
			{
				return null;
			}

			image.Position = 0;
			var bmpDecoder = new BmpDecoder();
			return bmpDecoder.Decode(Configuration.Default, image);
		}

		/// <summary>
		/// Fills a rectangle in the <see cref="PdfiumBitmap"/> with <paramref name="color"/>.
		/// The pixel values in the rectangle are replaced and not blended.
		/// </summary>
		public void FillRectangle(int left, int top, int width, int height, FPDF_COLOR color) => 
			Pdfium.FPDFBitmap_FillRect(Handle, left, top, width, height, color);

		/// <summary>
		/// Exposes the underlying image data directly as read-only stream in the
		/// <see href="https://en.wikipedia.org/wiki/BMP_file_format">BMP</see> file format.
		/// </summary>
		public Stream AsBmpStream(double dpiX = 72, double dpiY = 72) => 
			new BmpStream(this, dpiX, dpiY);

		/// <summary>
		/// Fills the whole <see cref="PdfiumBitmap"/> with <paramref name="color"/>.
		/// The pixel values in the rectangle are replaced and not blended.
		/// </summary>
		/// <param name="color"></param>
		public void Fill(FPDF_COLOR color) => 
			FillRectangle(0, 0, Width, Height, color);

		public void Dispose() => 
			((IDisposable)this).Dispose();

		protected override void Dispose(FPDF_BITMAP handle) => 
			Pdfium.FPDFBitmap_Destroy(handle);

		private static int GetBytesPerPixel(BitmapFormats format) => format switch
		{
			BitmapFormats.RGB => 3,
			BitmapFormats.RGBA => 4,
			BitmapFormats.RGBx => 4,
			BitmapFormats.Gray => 1,
			_ => throw new ArgumentOutOfRangeException(nameof(format))
		};

		private class BmpStream : Stream
		{
			private const uint BmpHeaderSize = 14;
			private const uint DibHeaderSize = 108; // BITMAPV4HEADER
			private const uint PixelArrayOffset = BmpHeaderSize + DibHeaderSize;
			private const uint CompressionMethod = 3; // BI_BITFIELDS
			private const uint MaskR = 0x00_FF_00_00;
			private const uint MaskG = 0x00_00_FF_00;
			private const uint MaskB = 0x00_00_00_FF;
			private const uint MaskA = 0xFF_00_00_00;

			private readonly PdfiumBitmap _bitmap;
			private readonly byte[] _header;
			private readonly uint _length;
			private readonly uint _stride;
			private readonly uint _rowLength;
			private uint _pos;

			public BmpStream(PdfiumBitmap bitmap, double dpiX, double dpiY)
			{
				if (bitmap.Format == BitmapFormats.Gray)
				{
					throw new NotSupportedException($"Bitmap format {bitmap.Format} is not yet supported.");
				}

				_bitmap = bitmap;
				_rowLength = (uint)bitmap.BytesPerPixel * (uint)bitmap.Width;
				_stride = ((uint)bitmap.BytesPerPixel * 8 * (uint)bitmap.Width + 31) / 32 * 4;
				_length = PixelArrayOffset + _stride * (uint)bitmap.Height + 1;
				_header = GetHeader(_length, _bitmap, dpiX, dpiY);
				_pos = 0;
			}

			private static byte[] GetHeader(uint fileSize, PdfiumBitmap bitmap, double dpiX, double dpiY)
			{
				const double metersPerInch = 0.0254;
				var header = new byte[BmpHeaderSize + DibHeaderSize];

				using var ms = new MemoryStream(header);
				using var writer = new BinaryWriter(ms);
				writer.Write((byte)'B'); 
				writer.Write((byte)'M');
				writer.Write(fileSize);
				writer.Write(0u);
				writer.Write(PixelArrayOffset);
				writer.Write(DibHeaderSize);
				writer.Write(bitmap.Width);
				writer.Write(-bitmap.Height); // top-down image
				writer.Write((ushort)1);
				writer.Write((ushort)(bitmap.BytesPerPixel * 8));
				writer.Write(CompressionMethod);
				writer.Write(0);
				writer.Write((int)Math.Round(dpiX / metersPerInch));
				writer.Write((int)Math.Round(dpiY / metersPerInch));
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

			public override bool CanRead => true;

			public override bool CanSeek => true;

			public override bool CanWrite => false;

			public override long Length => _length;

			public override long Position
			{
				get => _pos;
				set => _pos = value < 0 || value >= _length ?  throw new ArgumentOutOfRangeException() : (uint)value;
			}

			public override void Flush() { }

			public override int Read(byte[] buffer, int offset, int count)
			{
				var bytesToRead = count;
				var returnValue = 0;
				if (_pos < PixelArrayOffset)
				{
					returnValue = Math.Min(count, (int)(PixelArrayOffset - _pos));
					Buffer.BlockCopy(_header, (int)_pos, buffer, offset, returnValue);
					_pos += (uint)returnValue;
					offset += returnValue;
					bytesToRead -= returnValue;
				}

				if (bytesToRead <= 0)
				{
					return returnValue;
				}

				bytesToRead = Math.Min(bytesToRead, (int)(_length - _pos));
				var idxBuffer = _pos - PixelArrayOffset;

				if (_stride == _bitmap.Stride)
				{
					Marshal.Copy(_bitmap.Scan0 + (int)idxBuffer, buffer, offset, bytesToRead);
					returnValue += bytesToRead;
					_pos += (uint)bytesToRead;
					return returnValue;
				}

				while (bytesToRead > 0)
				{
					var idxInStride = (int)(idxBuffer / _stride);
					var leftInRow = Math.Max(0, (int)_rowLength - idxInStride);
					var paddingBytes = (int)(_stride - _rowLength);
					var read = Math.Min(bytesToRead, leftInRow);
					if (read > 0)
					{
						Marshal.Copy(_bitmap.Scan0 + (int)idxBuffer, buffer, offset, read);
					}
					offset += read;
					idxBuffer += (uint)read;
					bytesToRead -= read;
					returnValue += read;
					read = Math.Min(bytesToRead, paddingBytes);
					for (var i = 0; i < read; i++)
					{
						buffer[offset + i] = 0;
					}
					offset += read;
					idxBuffer += (uint)read;
					bytesToRead -= read;
					returnValue += read;
				}
				_pos = PixelArrayOffset + idxBuffer;
				return returnValue;
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
					case SeekOrigin.Begin:
						Position = offset;
						break;
					case SeekOrigin.Current:
						Position += offset;
						break;
					case SeekOrigin.End:
						Position = Length + offset;
						break;
				}
				return Position;
			}

			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}
		}
    }
}