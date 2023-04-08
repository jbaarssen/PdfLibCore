using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using PdfLibCore;
using PdfLibCore.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Processing;

// ReSharper disable UnusedMember.Local

namespace ExampleApp
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private const int Max = 865;
        private const string Destination = "c:/temp/output";

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Start...");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await Task.WhenAll(
                Run("A17_FlightPlan.pdf", Path.Combine(Destination, "lightplan")),
                Run("sample-10-mb.pdf", Path.Combine(Destination, "sample-10")),
                Run("A17_FlightPlan.pdf", Path.Combine(Destination, "flightplan2")),
                Run("sample-images.pdf", Path.Combine(Destination, "sample-images")));

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            Console.WriteLine("RunTime " + $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}");
        }

        private static async Task Run(string name, string destination)
        {
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }
            Directory.CreateDirectory(destination!);

            Console.WriteLine($"Starting with {name}");
            await foreach (var page in GetImagesFromPdf(name))
            {
                using var image = await Image.LoadAsync(page.Item2, new BmpDecoder());
                var data = await ResizeAndSaveToJpeg(image);
                await File.WriteAllBytesAsync(Path.Combine(destination, $"{page.i}.jpeg"), data);
            }

            Console.WriteLine($"Done with {name}");
        }

        private static async IAsyncEnumerable<(int i, Stream)> GetImagesFromPdf(string name)
        {
            var input = DataProvider.GetEmbeddedResource(name);
            using var pdfDocument = new PdfDocument(input);

            Console.WriteLine($"Getting {pdfDocument.Pages.Count} images for {name}");

            foreach (var page in pdfDocument.Pages)
            {
                using var bitmap = new PdfiumBitmap(
                    (int) (page.Size.Width / 72 * 144D),
                    (int) (page.Size.Height / 72 * 144D),
                    true);
                page.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);
                yield return (page.Index, bitmap.AsBmpStream());
            }

            Console.WriteLine($"Done getting images for {name}");
        }

        private static async Task<byte[]> ResizeAndSaveToJpeg(Image image)
        {
            var isPortrait = image.Width <= image.Height;
            using var clone = image.Clone(src => src.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = isPortrait
                    ? new Size(Max, 0)
                    : new Size(0, Max),
                Sampler = KnownResamplers.Lanczos3,
                Compand = true
            }));

            using var ms = new MemoryStream();
            await clone.SaveAsJpegAsync(ms);
            return ms.ToArray();
        }
    }
}