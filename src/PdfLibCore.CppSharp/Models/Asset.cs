using System;
using System.Text.Json.Serialization;

namespace PdfLibCore.CppSharp.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
public class Asset : GithubModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("label")]
    public string Label { get; set; } = null!;

    [JsonPropertyName("uploader")]
    public User Uploader { get; set; } = null!;

    [JsonPropertyName("content_type")]
    public string ContentType { get; set; } = null!;

    [JsonPropertyName("state")]
    public string State { get; set; } = null!;

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("download_count")]
    public int DownloadCount { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string BrowserDownloadUrl { get; set; } = null!;
}