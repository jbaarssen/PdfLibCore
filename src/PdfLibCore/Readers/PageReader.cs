using System;
using System.Drawing;
using System.Drawing.Imaging;
using PdfLibCore.Bindings;
using PdfLibCore.Models;

namespace PdfLibCore.Readers
{
    internal sealed class PageReader : IPageReader
    {
        private readonly FpdfPageT _page;
        private readonly double _nativeWidth;
        private readonly double _nativeHeight;
        private readonly float _depth;

        /// <inheritdoc />
        public int PageIndex { get; }

        public PageReader(DocumentWrapper docWrapper, int pageIndex, float depth = 96F)
        {
            PageIndex = pageIndex;
            _depth = depth;

            lock (PdfLib.Lock)
            {
                _page = fpdf_view.FPDF_LoadPage(docWrapper.Instance, pageIndex);

                // Get page size
                fpdf_view.FPDF_GetPageSizeByIndex(docWrapper.Instance, pageIndex, ref _nativeWidth, ref _nativeHeight);

                if (_page == null)
                {
                    throw new Exception($"failed to open page for page index {pageIndex}");
                }
            }
        }

        /// <inheritdoc />
        public double GetPageWidth()
        {
            return _nativeWidth;
        }

        /// <inheritdoc />
        public double GetPageHeight()
        {
            return _nativeHeight;
        }


        /// <inheritdoc />
        public byte[] GetImage()
        {
            lock (PdfLib.Lock)
            {
                var pageWidth = 0;
                var pageHeight = 0;
                Bitmap bitmap = null;
                BitmapData bitmapData = null;
                byte[] image;
                
                try
                {
                    pageWidth = (int) (300 * _nativeWidth / _depth);
                    pageHeight = (int) (300 * _nativeHeight / _depth);

                    bitmap = new Bitmap(pageWidth, pageHeight, PixelFormat.Format32bppArgb);
                    bitmapData = bitmap.LockBits(new Rectangle(0, 0, pageWidth, pageHeight), ImageLockMode.ReadWrite,
                        bitmap.PixelFormat);

                    var bitmapPtr =
                        fpdf_view.FPDFBitmapCreateEx(pageWidth, pageHeight, 4, bitmapData.Scan0, pageWidth * 4);
                    try
                    {
                        fpdf_view.FPDFBitmapFillRect(bitmapPtr, 0, 0, pageWidth, pageHeight, 0xFFFFFFFF);
                        fpdf_view.FPDF_RenderPageBitmap(bitmapPtr, _page, 0, 0, pageWidth, pageHeight,
                            (int) PdfRotation.Rotate0, (int) FPDF.CORRECT_FROM_DPI);
                    }
                    finally
                    {
                        fpdf_view.FPDFBitmapDestroy(bitmapPtr);
                    }
                }
                catch
                {
                    throw new Exception($"Failed to get page data for page {PageIndex} with size ({pageWidth} x {pageHeight})");
                }
                finally
                {
                    bitmap?.UnlockBits(bitmapData);
                    image = bitmap?.ToByteArray();
                    bitmap?.Dispose();
                }

                return image;
            }
        }

        public void Dispose()
        {
            lock (PdfLib.Lock)
            {
                if (_page == null)
                {
                    return;
                }

                fpdf_view.FPDF_ClosePage(_page);
            }
        }
    }
}