using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace magic_backend.Models;

public class BoxVM
{
    [JsonPropertyName("key")] 
    public int Key { get; set; }
    [JsonPropertyName("x")]
    public int X { get; set; }
    [JsonPropertyName("y")]
    public int Y { get; set; }
    [JsonPropertyName("color")]
    [Required]
    [Length(4,7)]
    public required string Color { get; set; }
    [JsonPropertyName("row")]
    public int Row { get; set; }
    [JsonPropertyName("isNewLayer")]
    public bool IsNewLayer { get; set; }
}