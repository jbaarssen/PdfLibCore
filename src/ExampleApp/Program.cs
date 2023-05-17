using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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

            if (true)
            {
                foreach (var resource in DataProvider.GetManifestResourceNames())
                {
                    if (!resource.StartsWith("Tests") || !Path.GetExtension(resource).EndsWith("pdf"))
                    {
                        continue;
                    }
                    await Run(resource, Path.Combine(Destination, Path.GetFileNameWithoutExtension(resource)));
                }
            }

            await Run("Tests.Bookmarks.pdf", Path.Combine(Destination, "Tests.Bookmarks"));

            //await Run("pdf-example-bookmarks.pdf", Path.Combine(Destination, "pdf-example-bookmarks"));

            //await Task.WhenAll(
            //    //Run("A17_FlightPlan.pdf", Path.Combine(Destination, "flightplan"))); //,
            //    //Run("sample-10-mb.pdf", Path.Combine(Destination, "sample-10")),
            //    //Run("A17_FlightPlan.pdf", Path.Combine(Destination, "flightplan2")),
            //    Run("pdf-example-bookmarks.pdf", Path.Combine(Destination, "pdf-example-bookmarks")),
            //    Run("sample-images.pdf", Path.Combine(Destination, "sample-images")));

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
                if (page.Item2 == null)
                {
                    continue;
                }
                using var image = await Image.LoadAsync(page.Item2, new BmpDecoder());
                var data = await ResizeAndSaveToJpeg(image);
                await File.WriteAllBytesAsync(Path.Combine(destination, $"{page.i}.jpeg"), data);
            }

            Console.WriteLine($"Done with {name}");
        }

        private static async IAsyncEnumerable<(int i, Stream)> GetImagesFromPdf(string name)
        {
            using var input = await DataProvider.GetEmbeddedResourceAsStreamAsync(name);
            using var pdfDocument = new PdfDocument(input);

            Console.WriteLine($"Getting {pdfDocument.Pages.Count} images for {name}");
            Console.WriteLine($"DuplexType: {pdfDocument.DuplexType}");
            Console.WriteLine($"Permissions: {pdfDocument.Permissions.GetValues()}");
            Console.WriteLine($"FileVersion: {pdfDocument.FileVersion}");
            Console.WriteLine($"PageMode: {pdfDocument.PageMode}");
            Console.WriteLine($"Destinations: {pdfDocument.Destinations.Count}");
            foreach (var destination in pdfDocument.Destinations)
            {
                Console.WriteLine($"Destination name: {destination.Name}");
                Console.WriteLine($"Destination pageIndex: {destination.PageIndex}");
                Console.WriteLine($"Destination locationInPage: {destination.LocationInPage}");
            }
            Console.WriteLine($"Bookmarks: {pdfDocument.Bookmarks.Count()}");

            void BookmarkLister(IEnumerable<PdfBookmark> bookmarks, int level)
            {
                var i = 1;
                foreach (var bookmark in bookmarks)
                {
                    Console.WriteLine($"{"".PadLeft(level)}Bookmark {i} title: {bookmark.Title}");
                    Console.WriteLine($"{"".PadLeft(level)}Bookmark {i} destination name: {bookmark.Destination.Name}");
                    Console.WriteLine($"{"".PadLeft(level)}Bookmark {i} destination pageIndex: {bookmark.Destination.PageIndex}");
                    Console.WriteLine($"{"".PadLeft(level)}Bookmark {i} destination locationInPage: {bookmark.Destination.LocationInPage}");
                    Console.WriteLine($"{"".PadLeft(level)}Bookmark {i} action: {bookmark.Action}");
                    BookmarkLister(bookmark.Children, level + 2);
                    i++;
                }
            }

            BookmarkLister(pdfDocument.Bookmarks, 0);

            Console.WriteLine($"Author: {pdfDocument.GetMetaText(MetadataTags.Author)}");
            Console.WriteLine($"Creator: {pdfDocument.GetMetaText(MetadataTags.Creator)}");
            Console.WriteLine($"Keywords: {pdfDocument.GetMetaText(MetadataTags.Keywords)}");
            Console.WriteLine($"Title: {pdfDocument.GetMetaText(MetadataTags.Title)}");
            Console.WriteLine($"Producer: {pdfDocument.GetMetaText(MetadataTags.Producer)}");
            Console.WriteLine($"Subject: {pdfDocument.GetMetaText(MetadataTags.Subject)}");
            Console.WriteLine($"CreationDate: {pdfDocument.GetMetaText(MetadataTags.CreationDate)}");
            Console.WriteLine($"ModDate: {pdfDocument.GetMetaText(MetadataTags.ModDate)}");

            foreach (var page in pdfDocument.Pages)
            {
                Console.WriteLine($"{name}, Page {page.Index} height: {page.Height}");
                Console.WriteLine($"{name}, Page {page.Index} width: {page.Width}");
                Console.WriteLine($"{name}, Page {page.Index} size: {page.Size}");
                Console.WriteLine($"{name}, Page {page.Index} label: {page.Label}");
                Console.WriteLine($"{name}, Page {page.Index} orientation: {page.Orientation}");
                Console.WriteLine($"{name}, Page {page.Index} hasTransparency: {page.HasTransparency}");

                yield return (page.Index, null);

                //using var bitmap = new PdfiumBitmap(
                //    (int) (page.Size.Width / 72 * 144D),
                //    (int) (page.Size.Height / 72 * 144D),
                //    true);
                //page.Render(bitmap, PageOrientations.Normal, RenderingFlags.LcdText);
                //yield return (page.Index, bitmap.AsBmpStream());
            }

            Console.WriteLine($"Done getting images for {name}");
        }

        private static async Task<byte[]> ResizeAndSaveToJpeg(Image image)
        {
            using var clone = image.Clone(src => src.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = src.GetCurrentSize().Width <= src.GetCurrentSize().Height
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