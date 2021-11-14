using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using PdfLibCore;
using PdfLibCore.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ExampleApp
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var destination = Path.Combine("c:/temp/output");
            
            using var pdfDocument = new PdfDocument(File.Open("c:/temp/IR.pdf", FileMode.Open));
            //using var pdfDocument = new PdfDocument(File.ReadAllBytes("c:/temp/IR.pdf"));

            var i = 0;
            foreach (var page in pdfDocument.Pages)
            {
                using var pdfPage = page;
                var pageWidth = (int) (300 * pdfPage.Size.Width / 72);
                var pageHeight = (int) (300 * pdfPage.Size.Height / 72);

                using var bitmap = new PdfiumBitmap(pageWidth, pageHeight, true);
                pdfPage.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);

                i++;
                SaveUsingImageSharp(bitmap, destination, i);
                SaveUsingMagickNet(bitmap, destination, i);
                SaveUsingSkiaSharp(bitmap, destination, i);
                SaveUsingFreeImage(bitmap, destination, i);

                //ResizeAndSaveToJpeg(bitmap.AsImage(196D, 196D), Path.Combine(destination, $"{i++}.jpeg"));
            }
            
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            Console.WriteLine("RunTime " + $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}");
            //Console.ReadKey();
        }

        private static void SaveUsingImageSharp(PdfiumBitmap bitmap, string destination, int i)
        {
            using var ms = new MemoryStream();
            PdfLibCore.ImageSharp.PdfiumBitmapExtensions.AsImage(bitmap, 196D, 196D).SaveAsJpeg(ms);
            File.WriteAllBytes(Path.Combine(destination, $"{i}-ImageSharp.jpeg"), ms.ToArray());
        }
        
        private static void SaveUsingMagickNet(PdfiumBitmap bitmap, string destination, int i)
        {
            using var ms = new MemoryStream();
            PdfLibCore.ImageSharp.PdfiumBitmapExtensions.AsImage(bitmap, 196D, 196D).SaveAsJpeg(ms);
            File.WriteAllBytes(Path.Combine(destination, $"{i}-MagickNet.jpeg"), ms.ToArray());
        }
        
        private static void SaveUsingSkiaSharp(PdfiumBitmap bitmap, string destination, int i)
        {
            using var ms = new MemoryStream();
            PdfLibCore.ImageSharp.PdfiumBitmapExtensions.AsImage(bitmap, 196D, 196D).SaveAsJpeg(ms);
            File.WriteAllBytes(Path.Combine(destination, $"{i}-SkiaSharp.jpeg"), ms.ToArray());
        }
        
        private static void SaveUsingFreeImage(PdfiumBitmap bitmap, string destination, int i)
        {
            using var ms = new MemoryStream();
            PdfLibCore.ImageSharp.PdfiumBitmapExtensions.AsImage(bitmap, 196D, 196D).SaveAsJpeg(ms);
            File.WriteAllBytes(Path.Combine(destination, $"{i}-FreeImage.jpeg"), ms.ToArray());
        }

        public static void ResizeAndSaveToJpeg(Image image, string destination)
        {
            var width = (int) (image.Width * 0.5D);
            var height = (int) (image.Height * 0.5D);
            image.Mutate(context => context.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(width, height)
            }));
            
            using var ms = new MemoryStream();
            image.SaveAsJpeg(ms);
            image.Dispose();
            
            File.WriteAllBytes(destination, ms.ToArray());
        }
    }
}