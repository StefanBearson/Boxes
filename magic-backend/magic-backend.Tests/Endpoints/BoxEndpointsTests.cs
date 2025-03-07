using FluentValidation;
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
        mockBoxService.RemoveAllBoxes().Returns(Task.CompletedTask);

        //Act
        var result = await BoxEndpoints.RemoveAllBoxes(mockBoxService);

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
        var okResult = Assert.IsType<Ok<BoxDTO>>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(box.Color, okResult.Value.Color);
    }
}