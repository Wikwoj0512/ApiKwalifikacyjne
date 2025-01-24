using System.Text.Json.Serialization;

namespace ApiKwalifikacyjne.Models;

public class ApiResponse
{
    [JsonPropertyName("items")] public IEnumerable<TagInfo> Items { get; set; }

    [JsonPropertyName("has_more")] public bool HasMore { get; set; }

    [JsonPropertyName("backoff")] public int? Backoff { get; set; }
}