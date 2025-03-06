using System.ComponentModel.DataAnnotations;

namespace magic_backend.Models;

public class BoxVM
{
    [Required]
    [MaxLength(10)]
    public int Key { get; set; }
    [Required]
    [MaxLength(50)]
    public int X { get; set; }
    [Required]
    [MaxLength(50)]
    public int Y { get; set; }
    [Required]
    [MaxLength(7)]
    public required string Color { get; set; }
    [Required]
    [MaxLength(2)]
    public int Row { get; set; }
    [Required]
    public bool IsNewLayer { get; set; }
    [Required]
    public int CurrentPositionX { get; set; }
    [Required]
    public int CurrentPositionY { get; set; }
}