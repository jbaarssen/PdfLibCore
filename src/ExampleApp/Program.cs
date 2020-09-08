using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using PdfLibCore;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            //var destination = Path.Combine("/Users/jan/Projects/temp/output");
            // Get bytes from PDF
            var webClient = new WebClient();
            var bytes = webClient.DownloadData("https://www.hq.nasa.gov/alsj/a17/A17_FlightPlan.pdf");
            
            using var pdfDocument = new PdfDocument(bytes, 0);

            int i = 0;
            foreach (var page in pdfDocument.Pages)
            {
                using (page)
                {
                    using (var bitmap = new PdfiumBitmap((int)page.Width, (int)page.Height, true))
                    using (var stream = new FileStream($"{i++}.bmp", FileMode.Create))
                    {
                        page.Render(bitmap);
                        bitmap.Save(stream);
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
    }
}