using System.Collections;
using System.Text.Json.Serialization;
using ApiKwalifikacyjne.Entities;

namespace ApiKwalifikacyjne.Models;

public class TagInfo
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("count")] public int Count { get; set; }

    [JsonPropertyName("has_synonyms")] public bool HasSynonyms { get; set; }

    [JsonPropertyName("is_moderator_only")]
    public bool IsModeratorOnly { get; set; }

    [JsonPropertyName("is_required")] public bool IsRequired { get; set; }
}