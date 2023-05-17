using System;
using System.IO;
using System.Linq;

namespace PdfLibCore.CppSharp.Models;

internal sealed class LibraryInfo
{
    public string ExtractedLibBaseDirectory { get; }
    public Uri BrowserDownloadUrl { get; }

    public LibraryInfo(string baseDestination, Release releaseInfo, string packageName = "pdfium-win-x64")
    {
        BrowserDownloadUrl = new Uri(releaseInfo.Assets.First(a => a.Name.Contains(packageName, StringComparison.OrdinalIgnoreCase)).BrowserDownloadUrl);
        ExtractedLibBaseDirectory = Path.Combine(baseDestination, Path.GetFileNameWithoutExtension(BrowserDownloadUrl.LocalPath));

        if (Directory.Exists(ExtractedLibBaseDirectory))
        {
            Directory.Delete(ExtractedLibBaseDirectory, true);
        }
        Directory.CreateDirectory(ExtractedLibBaseDirectory);
    }
}