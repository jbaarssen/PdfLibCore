using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using PdfLibCore.Enums;
using PdfLibCore.Types;

namespace PdfLibCore
{
    /// <summary>
	/// A bitmap to which a <see cref="PdfPage"/> can be rendered.
	/// </summary>
    public sealed class PdfiumBitmap : NativeWrapper<FPDF_BITMAP>
    {
	    private Bitmap _bitmap;
	    
		public int Width => Pdfium.FPDFBitmap_GetWidth(Handle);
		public int Height => Pdfium.FPDFBitmap_GetHeight(Handle);
		public int Stride => Pdfium.FPDFBitmap_GetStride(Handle);
		public IntPtr Scan0 => Pdfium.FPDFBitmap_GetBuffer(Handle);
		private readonly bool _forceAlphaChannel;

		public BitmapFormats Format => Pdfium.FPDFBitmap_GetFormat(Handle);

		PdfiumBitmap(FPDF_BITMAP bitmap)
			: base(bitmap)
		{
			if (bitmap.IsNull)
				throw new Exception();
		}
		
		~PdfiumBitmap()
		{
		}

		public Image Image
		{
			get
			{
				if (_bitmap != null)
				{
					return (Image) _bitmap;
				}

				if (IsDisposed)
				{
					throw new ObjectDisposedException(nameof(PdfiumBitmap));
				}

				PixelFormat pixelFormat;
				switch (Format)
				{
					case BitmapFormats.Gray:
					case BitmapFormats.RGB:
						throw new PdfiumException();
					case BitmapFormats.RGBx:
						pixelFormat = _forceAlphaChannel ? PixelFormat.Format32bppArgb : PixelFormat.Format32bppRgb;
						break;
					case BitmapFormats.RGBA:
						pixelFormat = PixelFormat.Format32bppArgb;
						break;
					default:
						throw new PdfiumException();
				}
				
				try
				{
					_bitmap = new Bitmap(Width, Height, Stride, pixelFormat, 
						Pdfium.FPDFBitmap_GetBuffer(Handle));
				}
				catch (Exception)
				{
					throw;
				}
				
				return _bitmap;
			}
		}

		/// <summary>
		/// Creates a new <see cref="PdfiumBitmap"/>. Unmanaged memory is allocated which must
		/// be freed by calling <see cref="Dispose"/>.
		/// </summary>
		/// <param name="width">The width of the new bitmap.</param>
		/// <param name="height">The height of the new bitmap.</param>
		/// <param name="hasAlpha">A value indicating wheter the new bitmap has an alpha channel.</param>
		/// <remarks>
		/// A bitmap created with this overload always uses 4 bytes per pixel.
		/// Depending on <paramref name="hasAlpha"/> the <see cref="Format"/> is then either
		/// <see cref="BitmapFormats.BGRA"/> or <see cref="BitmapFormats.BGRx"/>.
		/// </remarks>
		public PdfiumBitmap(int width, int height, bool hasAlpha, bool forceAlphaChannel = false)
			: this(Pdfium.FPDFBitmap_Create(width, height, hasAlpha))
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
			: this(Pdfium.FPDFBitmap_CreateEx(width, height, format, scan0, stride))
		{
			FillRectangle(0, 0, width, height, 0xFFFFFFFF);
		}

		/// <summary>
		/// Fills a rectangle in the <see cref="PdfiumBitmap"/> with <paramref name="color"/>.
		/// The pixel values in the rectangle are replaced and not blended.
		/// </summary>
		public void FillRectangle(int left, int top, int width, int height, FPDF_COLOR color)
		{
			Pdfium.FPDFBitmap_FillRect(Handle, left, top, width, height, color);
		}

		/// <summary>
		/// Fills the whole <see cref="PdfiumBitmap"/> with <paramref name="color"/>.
		/// The pixel values in the rectangle are replaced and not blended.
		/// </summary>
		/// <param name="color"></param>
		public void Fill(FPDF_COLOR color) => FillRectangle(0, 0, Width, Height, color);

		public void Dispose()
		{
			((IDisposable)this).Dispose();
			_bitmap?.Dispose();
			_bitmap = null;
		}
		
		protected override void Dispose(FPDF_BITMAP handle)
		{
			Pdfium.FPDFBitmap_Destroy(handle);
		}
    }
}