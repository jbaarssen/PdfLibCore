using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FluentAssertions;
using PdfLibCore.Enums;
using Xunit;

namespace PdfLibCore.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class PdfDocumentTests
    {
        private readonly string _path;

        public PdfDocumentTests()
        {
            _path = Path.Combine(Environment.CurrentDirectory, "Data", "test.pdf");
        }

        [Fact]
        public void Create_New_PdfDocument()
        {
            using var pdfDocument = new PdfDocument();
            pdfDocument.Should().NotBeNull();
            pdfDocument.Pages.Count.Should().Be(0);
        }
        
        [Fact]
        public void Open_PdfDocument_Filepath()
        {
            using var pdfDocument = new PdfDocument(_path);
            pdfDocument.Should().NotBeNull();
            pdfDocument.Pages.Count.Should().Be(1);
        }
        
        [Fact]
        public void Open_PdfDocument_Filepath_Non_Existant_File()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Data", "test2.pdf");
            Assert.Throws<PdfiumException>(() => new PdfDocument(path)); 
        }
        
        [Fact]
        public void Open_PdfDocument_Stream()
        {
            using var stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            using var pdfDocument = new PdfDocument(stream);
            pdfDocument.Should().NotBeNull();
            pdfDocument.Pages.Count.Should().Be(1);
        }
        
        [Fact]
        public void Open_PdfDocument_Bytes()
        {
            using var pdfDocument = new PdfDocument(File.ReadAllBytes(_path));
            pdfDocument.Should().NotBeNull();
            pdfDocument.Pages.Count.Should().Be(1);
        }
        
        [Fact(Skip = "PageRange not present...")]
        public void Get_PageRange_From_Document()
        {
            using var pdfDocument = new PdfDocument(_path);
            
            pdfDocument.PageRange.Should().NotBeNull();
            pdfDocument.PageRange.PrintRangeElement(0).Should().Be(0);
            pdfDocument.PageRange.PrintPageRangeCount.Should().Be(1);
        }
        
        [Fact]
        public void Get_PageMode_From_Document()
        {
            using var pdfDocument = new PdfDocument(_path);
            pdfDocument.PageMode.Should().Be(PageModes.UseNone);
        }
        
        [Fact]
        public void Get_Page_From_Document()
        {
            using var pdfDocument = new PdfDocument(_path);
            pdfDocument.Pages.Should().HaveCount(1);
            pdfDocument.Pages.Count.Should().Be(1);
            
            using var page = pdfDocument.Pages[0];
            page.Should().NotBeNull();
            page.Width.Should().BeApproximately(595D, 1D);
            page.Height.Should().BeApproximately(841D, 1D);
            page.Size.Width.Should().BeApproximately(595D, 1D);
            page.Size.Height.Should().BeApproximately(841D, 1D);
            page.Orientation.Should().Be(PageOrientations.Normal);
            page.HasTransparency.Should().BeFalse();
            page.Label.Should().BeEquivalentTo(string.Empty);
        }
    }
}