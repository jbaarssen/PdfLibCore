using System;
using System.IO;
using PdfLibCore.Enums;
using PdfLibCore.Types;

// ReSharper disable MemberCanBePrivate.Global
namespace PdfLibCore
{
	/// <summary>
	/// A bitmap to which a <see cref="PdfPage"/> can be rendered.
	/// </summary>
	public sealed class PdfiumBitmap : NativeWrapper<FPDF_BITMAP>
	{
		public int Width => Pdfium.FPDFBitmap_GetWidth(Handle);
		public int Height => Pdfium.FPDFBitmap_GetHeight(Handle);
		public int Stride => Pdfium.FPDFBitmap_GetStride(Handle);
		public IntPtr Scan0 => Pdfium.FPDFBitmap_GetBuffer(Handle);
		public int BytesPerPixel => GetBytesPerPixel(Format);
		public BitmapFormats Format { get; }

		private PdfiumBitmap(FPDF_BITMAP bitmap, BitmapFormats format)
			: base(bitmap.IsNull ? throw new Exception() : bitmap) =>
			GetBytesPerPixel(Format = format);

		/// <summary>
		/// Creates a new <see cref="PdfiumBitmap"/>. Unmanaged memory is allocated which must
		/// be freed by calling <see cref="Dispose()"/>.
		/// </summary>
		/// <param name="width">The width of the new bitmap.</param>
		/// <param name="height">The height of the new bitmap.</param>
		/// <param name="hasAlpha">A value indicating wheter the new bitmap has an alpha channel.</param>
		/// <remarks>
		/// A bitmap created with this overload always uses 4 bytes per pixel.
		/// Depending on <paramref name="hasAlpha"/> the <see cref="Format"/> is then either
		/// <see cref="BitmapFormats.RGBA"/> or <see cref="BitmapFormats.RGBx"/>.
		/// </remarks>
		public PdfiumBitmap(int width, int height, bool hasAlpha)
			: this(Pdfium.FPDFBitmap_Create(width, height, hasAlpha), hasAlpha ? BitmapFormats.RGBA : BitmapFormats.RGBx) =>
			FillRectangle(0, 0, width, height, 0xFFFFFFFF);

		/// <summary>
		/// Creates a new <see cref="PdfiumBitmap"/> using memory allocated by the caller.
		/// The caller is responsible for freeing the memory and that the adress stays
		/// valid during the lifetime of the returned <see cref="PdfiumBitmap"/>. To free
		/// unmanaged resources, <see cref="Dispose()"/> must be called.
		/// </summary>
		/// <param name="width">The width of the new bitmap.</param>
		/// <param name="height">The height of the new bitmap.</param>
		/// <param name="format">The format of the new bitmap.</param>
		/// <param name="scan0">The adress of the memory block which holds the pixel values.</param>
		/// <param name="stride">The number of bytes per image row.</param>
		public PdfiumBitmap(int width, int height, BitmapFormats format, IntPtr scan0, int stride)
			: this(Pdfium.FPDFBitmap_CreateEx(width, height, format, scan0, stride), format) =>
			FillRectangle(0, 0, width, height, 0xFFFFFFFF);

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
		public Stream AsBmpStream(double dpiX = 72, double dpiY = 72)
		{
			var stream = new BmpStream(this, dpiX, dpiY);
			stream.Position = 0;
			return stream;
		}

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
	}
}