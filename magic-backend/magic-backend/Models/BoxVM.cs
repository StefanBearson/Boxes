using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace magic_backend.Models;

public class BoxVM
{
    [JsonPropertyName("key")] 
    public int? Key { get; set; } = 0;
    [JsonPropertyName("x")]
    public int? X { get; set; } = 0;
    [JsonPropertyName("y")]
    public int? Y { get; set; } = 0;
    [JsonPropertyName("color")]
    public required string Color { get; set; }
    [JsonPropertyName("row")]
    public int? Row { get; set; } = 0;
    [JsonPropertyName("isNewLayer")]
    public bool? IsNewLayer { get; set; }
}