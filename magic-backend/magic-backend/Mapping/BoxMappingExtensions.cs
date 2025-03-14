using magic_backend.Models;

namespace magic_backend.Mapping;

public static class BoxMappingExtensions
{
    public static BoxDTO ToDTO(this BoxVM box)
    {
        return new BoxDTO
        {
            Key = box.Key,
            X = box.X,
            Y = box.Y,
            Color = box.Color.Trim(),
            Row = box.Row,
            IsNewLayer = box.IsNewLayer 
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
            IsNewLayer = box.IsNewLayer
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
            IsNewLayer = box.IsNewLayer
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
            IsNewLayer = box.IsNewLayer
        };
    }
}