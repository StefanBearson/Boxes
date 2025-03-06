using magic_backend.Models;

namespace magic_backend.Mapping;

public static class BoxMappingExtensions
{
    public static BoxDTO ToDTO(this BoxVM box)
    {
        return new BoxDTO
        {
            Key = box.Key ?? 0,
            X = box.X ?? 0,
            Y = box.Y ?? 0,
            Color = box.Color.Trim(),
            Row = box.Row ?? 0,
            IsNewLayer = box.IsNewLayer,
            CurrentPositionX = box.CurrentPositionX ?? 0,
            CurrentPositionY = box.CurrentPositionY ?? 0
        };
    }
    
    public static Box ToEntity(this BoxDTO box)
    {
        return new Box
        {
            Key = box.Key,
            X = box.X,
            Y = box.Y,
            Color = box.Color,
            Row = box.Row,
            IsNewLayer = box.IsNewLayer,
            CurrentPositionX = box.CurrentPositionX,
            CurrentPositionY = box.CurrentPositionY
        };
    }
    
    public static BoxVM ToVM(this BoxDTO box)
    {
        return new BoxVM
        {
            Key = box.Key,
            X = box.X,
            Y = box.Y,
            Color = box.Color,
            Row = box.Row,
            IsNewLayer = box.IsNewLayer,
            CurrentPositionX = box.CurrentPositionX,
            CurrentPositionY = box.CurrentPositionY
        };
    }
    
    public static BoxDTO ToDTO(this Box box)
    {
        return new BoxDTO
        {
            Key = box.Key,
            X = box.X,
            Y = box.Y,
            Color = box.Color,
            Row = box.Row,
            IsNewLayer = box.IsNewLayer,
            CurrentPositionX = box.CurrentPositionX,
            CurrentPositionY = box.CurrentPositionY
        };
    }
}