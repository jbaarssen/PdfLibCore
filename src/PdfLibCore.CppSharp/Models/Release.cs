using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace PdfLibCore.CppSharp.Models;

public class Release : GithubModel
{
    [JsonPropertyName("assets_url")]
    public string AssetsUrl { get; set; } = null!;

    [JsonPropertyName("upload_url")]
    public string UploadUrl { get; set; } = null!;

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = null!;

    [JsonPropertyName("tag_name")]
    public string TagName { get; set; } = null!;

    [JsonPropertyName("target_commitish")]
    public string TargetCommitish { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("draft")]
    public bool Draft { get; set; }

    [JsonPropertyName("author")]
    public User Author { get; set; } = null!;

    [JsonPropertyName("prerelease")]
    public bool Prerelease { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }

    [JsonPropertyName("assets")]
    public List<Asset> Assets { get; set; } = new();

    [JsonPropertyName("tarball_url")]
    public string TarballUrl { get; set; } = null!;

    [JsonPropertyName("zipball_url")]
    public string ZipballUrl { get; set; } = null!;

    [JsonPropertyName("body")]
    public string Body { get; set; } = null!;
}