using System;
using System.IO;
using System.Linq;
using System.Numerics;

namespace PdfLibCore.CppSharp.Models;

internal sealed class LibraryInfo : IDisposable
{
    public string ExtractedLibBaseDirectory { get; }
    public Uri BrowserDownloadUrl { get; }
    public string Filename { get; }
    public string FullFilePath { get; }

    public LibraryInfo(string baseDestination, Release releaseInfo, string packageName = "pdfium-win-x64")
    {
        BrowserDownloadUrl = new Uri(releaseInfo.Assets.First(a => a.Name.Contains(packageName, StringComparison.OrdinalIgnoreCase)).BrowserDownloadUrl);
        ExtractedLibBaseDirectory = Path.Combine(baseDestination, Path.GetFileNameWithoutExtension(BrowserDownloadUrl.LocalPath));
        Filename = Path.GetFileName(BrowserDownloadUrl.LocalPath);
        FullFilePath = Path.Combine(ExtractedLibBaseDirectory, Filename);
        Cleanup();
    }

    public void Dispose()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        if (Directory.Exists(ExtractedLibBaseDirectory))
        {
            foreach (var file in Directory.GetFiles(ExtractedLibBaseDirectory, "", SearchOption.AllDirectories))
            {
                if (Path.GetFileName(file).Equals(Filename))
                {
                    continue;
                }
                File.Delete(file);
            }

            CleanupEmptyDir(ExtractedLibBaseDirectory, false);
        }
        Directory.CreateDirectory(ExtractedLibBaseDirectory);
    }

    private static void CleanupEmptyDir(string directory, bool remove = true)
    {
        foreach (var subDirectory in Directory.GetDirectories(directory))
        {
            CleanupEmptyDir(subDirectory);
        }
        if (remove)
        {
            Directory.Delete(directory);
        }
    }
}