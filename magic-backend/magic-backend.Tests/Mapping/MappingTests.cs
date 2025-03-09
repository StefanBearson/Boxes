using magic_backend.Mapping;
using magic_backend.Models;

namespace magic_backend.Tests.Mapping;

public class MappingTests
{
    [Fact]
    public void VMToDTO_Mapping_ReturnsCorrectBoxDTO()
    {
        // Arrange
        var boxVM = new BoxVM
        {
            Key = 1,
            X = 10,
            Y = 20,
            Color = "Red ",
            Row = 2,
            IsNewLayer = true
        };

        // Act
        var result = boxVM.ToDTO();

        // Assert
        Assert.Equal(1, result.Key);
        Assert.Equal(10, result.X);
        Assert.Equal(20, result.Y);
        Assert.Equal("Red", result.Color);
        Assert.Equal(2, result.Row);
        Assert.True(result.IsNewLayer);
    }

    [Fact]
    public void VMToDTO_Mapping_HandlesNullValues()
    {
        // Arrange
        var boxVM = new BoxVM
        {
            Key = null,
            X = null,
            Y = null,
            Color = " Blue ",
            Row = null,
            IsNewLayer = null
        };

        // Act
        var result = boxVM.ToDTO();

        // Assert
        Assert.Equal(0, result.Key);
        Assert.Equal(0, result.X);
        Assert.Equal(0, result.Y);
        Assert.Equal("Blue", result.Color);
        Assert.Equal(0, result.Row);
        Assert.False(result.IsNewLayer);
    }
    
    [Fact]
    public void DTOToEntity_Mapping_ReturnsCorrectBox()
    {
        // Arrange
        var boxDTO = new BoxDTO
        {
            Key = 1,
            X = 10,
            Y = 20,
            Color = "Red",
            Row = 2,
            IsNewLayer = true
        };
        
        // Act
        var result = boxDTO.ToEntity();

        // Assert
        Assert.Equal(1, result.Key);
        Assert.Equal(10, result.X);
        Assert.Equal(20, result.Y);
        Assert.Equal("Red", result.Color);
        Assert.Equal(2, result.Row);
        Assert.True(result.IsNewLayer);
    }
    
    [Fact]
    public void DTOToVM_Mapping_ReturnsCorrectBoxVM()
    {
        // Arrange
        var boxDTO = new BoxDTO
        {
            Key = 1,
            X = 10,
            Y = 20,
            Color = "Red",
            Row = 2,
            IsNewLayer = true
        };
        
        // Act
        var result = boxDTO.ToVM();

        // Assert
        Assert.Equal(1, result.Key);
        Assert.Equal(10, result.X);
        Assert.Equal(20, result.Y);
        Assert.Equal("Red", result.Color);
        Assert.Equal(2, result.Row);
        Assert.True(result.IsNewLayer);
    }
    
    [Fact]
    public void EntityToDTO_Mapping_ReturnsCorrectBoxDTO()
    {
        // Arrange
        var box = new Box
        {
            Key = 1,
            X = 10,
            Y = 20,
            Color = "Red",
            Row = 2,
            IsNewLayer = true
        };

        // Act
        var result = box.ToDTO();

        // Assert
        Assert.Equal(1, result.Key);
        Assert.Equal(10, result.X);
        Assert.Equal(20, result.Y);
        Assert.Equal("Red", result.Color);
        Assert.Equal(2, result.Row);
        Assert.True(result.IsNewLayer);
    }
}