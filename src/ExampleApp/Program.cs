using System;
using System.Diagnostics;
using System.IO;
using PdfLibCore;
using PdfLibCore.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ExampleApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var destination = Path.Combine("c:/temp/output");
            // Get bytes from PDF
            var bytes = File.ReadAllBytes("c:/temp/IR.pdf");
            
            using var pdfDocument = new PdfDocument(bytes, 0);

            var i = 0;
            foreach (var page in pdfDocument.Pages)
            {
                using (page)
                {
                    var pageWidth = (int) (300 * page.Size.Width / 72);
                    var pageHeight = (int) (300 * page.Size.Height / 72);

                    using var bitmap = new PdfiumBitmap(pageWidth, pageHeight, true);
                    page.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);

                    using var ms = new MemoryStream();
                    bitmap.AsImage(196D, 196D).SaveAsJpeg(ms);
                    File.WriteAllBytes(Path.Combine(destination, $"{i++}.jpeg"), ms.ToArray());

                    //ResizeAndSaveToJpeg(bitmap.AsImage(196D, 196D), Path.Combine(destination, $"{i++}.jpeg"));
                }
            }
            
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            Console.WriteLine("RunTime " + $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}");
            Console.ReadKey();
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