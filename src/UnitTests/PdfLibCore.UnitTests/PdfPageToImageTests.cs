using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FluentAssertions;
using PdfLibCore.Enums;
using Xunit;

namespace PdfLibCore.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class PdfPageToImageTests : IDisposable
    {
        private readonly PdfDocument _pdfDocument;

        public PdfPageToImageTests()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Data", "test.pdf");
            _pdfDocument = new PdfDocument(path);
            _pdfDocument.Should().NotBeNull();
            _pdfDocument.Pages.Count.Should().Be(1);
        }

        [Fact]
        public void Render_Page_To_PdfiumBitmap()
        {
            var page = _pdfDocument.Pages[0];
            page.Should().NotBeNull();
            
            using var bitmap = new PdfiumBitmap((int)page.Width, (int)page.Height, true);
            page.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);
            bitmap.Format.Should().Be(BitmapFormats.RGBA);
            bitmap.Width.Should().Be((int)page.Width);
            bitmap.Height.Should().Be((int)page.Height);
            bitmap.Stride.Should().BeGreaterThanOrEqualTo(0);
            bitmap.Scan0.Should().BeGreaterThanOrEqualTo(0);
            bitmap.BytesPerPixel.Should().Be(4);
            bitmap.Dispose();
        }
        
        [Fact]
        public void Get_BmpStream_From_PdfiumBitmap()
        {
            var page = _pdfDocument.Pages[0];
            page.Should().NotBeNull();
            
            using var bitmap = new PdfiumBitmap((int)page.Width, (int)page.Height, true);
            page.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);
            var stream = bitmap.AsBmpStream();
            stream.Should().NotBeNull();
        }

        public void Dispose()
        {
            ((IDisposable)_pdfDocument)?.Dispose();
        }
    }
}