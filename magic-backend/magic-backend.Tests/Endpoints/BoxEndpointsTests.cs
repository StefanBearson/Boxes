using FluentValidation;
using FluentValidation.TestHelper;
using magic_backend.Endpoints;
using magic_backend.Filters;
using magic_backend.Models;
using magic_backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using NSubstitute;


namespace magic_backend.Tests.Endpoints;

public class BoxEndpointsTests
{
    private IValidator<BoxVM> _validator;
    private ILogger<ValidationFilter<BoxVM>> _logger;
    
    [Fact]
    public async Task RemoveAllBoxes_ReturnsOkResult()
    {
        //Arrange
        var mockBoxService = Substitute.For<IBoxService>();
        mockBoxService.DeleteAllBoxes().Returns(Task.FromResult("All boxes removed"));

        //Act
        var result = await BoxEndpoints.DeleteAllBoxes(mockBoxService);

        //Assert
        var okResult = Assert.IsType<Ok<string>>(result);
        Assert.Equal(200, okResult.StatusCode);

    }
    
    [Fact]
    public async Task GetBoxes_ReturnsOkResult()
    {
        //Arrange
        var mockBoxService = Substitute.For<IBoxService>();
        var expected = new List<BoxDTO> { new BoxDTO { Color = "#111111" } };
        mockBoxService.GetBoxes().Returns(Task.FromResult(expected));

        //Act
        var result = await BoxEndpoints.GetBoxes(mockBoxService);

        //Assert
        var okResult = Assert.IsType<Ok<IEnumerable<BoxVM>>>(result);
        Assert.Single(okResult.Value);
        Assert.Equal(expected.First().Color, okResult.Value.First().Color);
    }
    
    [Fact]
    public async Task CreateBox_ReturnsOkResult()
    {
        //Arrange
        var mockBoxService = Substitute.For<IBoxService>();
        var box = new BoxVM { Color = "red" };
        mockBoxService.CreateBox(Arg.Any<BoxDTO>()).Returns(Task.FromResult("Red"));
        
        //Act
        var result = await BoxEndpoints.CreateBox(box, mockBoxService);
        
        //Assert
        var okResult = Assert.IsType<Created<BoxDTO>>(result);
        Assert.Equal(201, okResult.StatusCode);
        if (okResult.Value != null) Assert.Equal(box.Color, okResult.Value.Color);
    }
    
    [Fact]
    public async Task CreateBox_UseNotValidModel_ReturnsNotValid()
    {
        //Arrange
        BoxEndpoints.AddBoxValidator validator = new BoxEndpoints.AddBoxValidator();
        BoxVM box = new BoxVM
        {
            Color = "", 
            IsNewLayer = true,
            Key = 0,
            Row = 0,
            X = 0,
            Y = 0
        };
        
        //Act
        var result = validator.TestValidate(box);
        
        //Assert
        Assert.False(result.IsValid);
        Assert.Equal("Color", result.Errors[0].PropertyName);
        Assert.Equal("'Color' must not be empty.", result.Errors[0].ErrorMessage);
        Assert.Equal("The length of 'Color' must be at least 4 characters. You entered 0 characters.", result.Errors[1].ErrorMessage);
    }
    
    [Fact]
    public async Task CreateBox_UseValidModel_ReturnsValidResult()
    {
        //Arrange
        BoxEndpoints.AddBoxValidator validator = new BoxEndpoints.AddBoxValidator();
        BoxVM box = new BoxVM
        {
            Color = "#111111", 
            IsNewLayer = true,
            Key = 0,
            Row = 0,
            X = 0,
            Y = 0
        };
        
        
        //Act
        var result = validator.TestValidate(box);
        
        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}