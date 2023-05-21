using System;
using System.IO;
using System.Linq;

namespace PdfLibCore.CppSharp.Models;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
public class RunnerOptions
{
    public string RootDirIndicatorFile { get; set; } = "README.md";
    public string PdfiumReleaseGithubUrl { get; set; } = "https://api.github.com/repos/bblanchon/pdfium-binaries/releases/";
    public string GitHubReleaseId { get; set; } = "latest";
    public Uri GithubDownloadUrl => new(new Uri(PdfiumReleaseGithubUrl), GitHubReleaseId);
    public int BuildRevision { get; set; } = 0;
    public string SolutionDir { get; set; }
    public string PdfiumGeneratedProjectDir { get; set; }
    public string DestinationLibraryPath { get; set; }
    public string RootDir { get; }
    public string GeneratedProjectName { get; set; } = "PdfLibCore.Generated";
    public string SharedLibraryName { get; set; } = "Pdfium";

    public RunnerOptions()
    {
        RootDir = GetRootDir();
        SolutionDir = Path.GetFullPath(Path.Combine(RootDir, "src", typeof(Program).Namespace!));
        PdfiumGeneratedProjectDir = Path.GetFullPath(Path.Combine(RootDir, "src", GeneratedProjectName));
        DestinationLibraryPath = Path.GetFullPath(Path.Combine(RootDir, "artifacts", "libraries"));

        Directory.CreateDirectory(DestinationLibraryPath);
    }

    private string GetRootDir()
    {
        var dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

        do
        {
            var files = dirInfo.GetFiles();
            if (files.Any(f => f.Name == RootDirIndicatorFile))
            {
                return dirInfo.FullName;
            }
            dirInfo = dirInfo.Parent;
        }
        while (dirInfo is { Exists: true });

        throw new DirectoryNotFoundException("Could not determine project root directory.");
    }

}