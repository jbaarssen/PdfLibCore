[![build](https://github.com/jbaarssen/PdfLibCore/actions/workflows/build-validation.yml/badge.svg)](https://github.com/jbaarssen/PdfLibCore/actions/workflows/build-validation.yml) 
[![Release Package Version](https://github.com/jbaarssen/PdfLibCore/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/jbaarssen/PdfLibCore/actions/workflows/nuget-publish.yml)

# PdfLibCore
PdfLib CORE is a fast PDF editing and reading library for modern .NET Core applications.

## Example: Creating images from all pages in a PDF

Opening a PDF file can be done either through providing a filepath, a stream or a byte array..

```c#
var dpiX, dpiY = 300D;
var i = 0;

using var pdfDocument = new PdfDocument(File.Open(<<file>>, FileMode.Open));
foreach (var page in pdfDocument.Pages)
{
    using var pdfPage = page;
    var pageWidth = (int) (dpiX * pdfPage.Size.Width / 72);
    var pageHeight = (int) (dpiY * pdfPage.Size.Height / 72);

    using var bitmap = new PdfiumBitmap(pageWidth, pageHeight, true);
    pdfPage.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);
    using var stream = bitmap.AsBmpStream(dpiX, dpiY);
    
    // <<< do something with your stream...>>> 
}
```

## Notes
If you encounter errors in T4 generation on Windows VS, convert the line endings to CRLF and it will work.