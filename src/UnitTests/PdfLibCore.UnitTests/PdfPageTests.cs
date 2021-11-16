using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FluentAssertions;
using PdfLibCore.Enums;
using Xunit;

namespace PdfLibCore.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class PdfPageTests
    {
        private readonly string _path;

        public PdfPageTests()
        {
            _path = Path.Combine(Environment.CurrentDirectory, "Data", "test.pdf");
        }
        
        [Fact]
        public void Add_New_Page_To_Collection_From_Different_pdf()
        {
            var srcDocument = new PdfDocument(_path);
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            pdfDocument.Pages.Add(srcDocument, 0).Should().BeTrue();
            pdfDocument.Pages.Should().HaveCount(1);
        }
        
        [Fact]
        public void Add_New_Page_To_Collection()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            pdfDocument.Pages.Add(595D, 841D).Should().NotBeNull();
            pdfDocument.Pages.Should().HaveCount(1);
            pdfDocument.Pages[0].Orientation.Should().Be(PageOrientations.Normal);
            pdfDocument.Pages[0].Orientation = PageOrientations.Rotated90CW;
            pdfDocument.Pages[0].Orientation.Should().Be(PageOrientations.Rotated90CW);
        }
        
        [Fact]
        public void Add_New_Page_To_Collection_And_Remove_It_Again()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            pdfDocument.Pages.Add(595D, 841D).Should().NotBeNull();
            pdfDocument.Pages.Should().HaveCount(1);
            pdfDocument.Pages.RemoveAt(0);
            pdfDocument.Pages.Should().HaveCount(0);
        }
        
        [Fact]
        public void Remove_Non_Existing_Page_Results_In_ArgumentOutOfRange_Exception()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            pdfDocument.Pages.Add(595D, 841D).Should().NotBeNull();
            pdfDocument.Pages.Should().HaveCount(1);
            Assert.Throws<ArgumentOutOfRangeException>(() => pdfDocument.Pages.RemoveAt(1));
        }
        
        [Fact]
        public void Add_New_Page_To_Collection_And_Remove_It_Again_By_Page()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            var page = pdfDocument.Pages.Add(595D, 841D);
            page.Should().NotBeNull();
            pdfDocument.Pages.Should().HaveCount(1);
            pdfDocument.Pages.Remove(page);
            pdfDocument.Pages.Should().HaveCount(0);
        }
        
         [Fact]
        public void Try_Get_Non_Existing_Page_Results_In_ArgumentOutOfRange_Exception()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            pdfDocument.Pages.Add(595D, 841D).Should().NotBeNull();
            pdfDocument.Pages.Should().HaveCount(1);
            Assert.Throws<ArgumentOutOfRangeException>(() => pdfDocument.Pages[1]);
        }
        
        [Fact]
        public void Render_Page()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            var page = pdfDocument.Pages.Add(595D, 841D);
            pdfDocument.Pages.Should().HaveCount(1);
            page.Should().NotBeNull();

            var bitmap = new PdfiumBitmap((int)page.Width, (int)page.Height, false);
            page.Render(bitmap);
            bitmap.Should().NotBeNull();
            bitmap.Format.Should().Be(BitmapFormats.RGBx);
            bitmap.Width.Should().Be((int)page.Width);
            bitmap.Height.Should().Be((int)page.Height);
        }
        
        [Fact]
        public void DeviceToPage()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            var page = pdfDocument.Pages.Add(595D, 841D);
            pdfDocument.Pages.Should().HaveCount(1);
            page.Should().NotBeNull();

            var result = page.DeviceToPage((0, 0, 10, 10), 0, 0);
            result.Should().NotBeNull();
            result.X.Should().Be(0);
            result.Y.Should().BeApproximately(841D, 1D);
        }
        
        [Fact]
        public void PageToDevice()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            var page = pdfDocument.Pages.Add(595D, 841D);
            pdfDocument.Pages.Should().HaveCount(1);
            page.Should().NotBeNull();

            var result = page.PageToDevice((0, 0, 10, 10), 0, 0);
            result.Should().NotBeNull();
            result.X.Should().Be(0);
            result.Y.Should().Be(10);
        }
        
        [Fact]
        public void Flatten()
        {
            var pdfDocument = new PdfDocument();
            pdfDocument.Pages.Should().HaveCount(0);
            var page = pdfDocument.Pages.Add(595D, 841D);
            pdfDocument.Pages.Should().HaveCount(1);
            page.Should().NotBeNull();

            var result = page.Flatten(FlattenFlags.NormalDisplay);
            result.Should().Be(FlattenResults.NothingToDo);
        }
    }
}