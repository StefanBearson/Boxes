namespace magic_backend.Models;

public class BoxDTO
{
    public int Key { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public required string Color { get; set; }
    public int Row { get; set; }
    public bool IsNewLayer { get; set; }
    public int CurrentPositionX { get; set; }
    public int CurrentPositionY { get; set; }
}