using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using PdfLibCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var destination = Path.Combine("/Users/jan/Projects/temp/output");
            // Get bytes from PDF
            var bytes = File.ReadAllBytes("/Users/jan/Downloads/sticker.pdf");
            
            using var pdfDocument = new PdfDocument(bytes, 0);

            int i = 0;
            foreach (var page in pdfDocument.Pages)
            {
                using (page)
                {
                    var pageWidth = (int) (300 * page.Size.Width / 96F);
                    var pageHeight = (int) (300 * page.Size.Height / 96F);
                    
                    using (var bitmap = new PdfiumBitmap(pageWidth, pageHeight, true))
                    {
                        page.Render(bitmap);
                        
                        //SaveToJpeg(bitmap.AsBmpStream(196D, 196D), Path.Combine(destination, $"{i++}.jpeg"));
                    }
                }
            }
            
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadKey();
        }

        public static void SaveToJpeg(Stream image, string destination)
        {
            if (image == null)
            {
                return;
            }
            
            image.Position = 0;
            var bmpDecoder = new BmpDecoder();
            var img = bmpDecoder.Decode(Configuration.Default, image);
            if (img == null)
            {
                return;
            }
            var width = (int) (img.Width * 0.5D);
            var height = (int) (img.Height * 0.5D);
            img.Mutate(context => context.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(width, height)
            }));
            
            using var ms = new MemoryStream();
            img.Save(ms, JpegFormat.Instance);
            img.Dispose();
            
            File.WriteAllBytes(destination, ms.ToArray());
        }
    }
}