using System.Text.Json.Serialization;

namespace PdfLibCore.CppSharp.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
public abstract class GithubModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;

    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = null!;
}