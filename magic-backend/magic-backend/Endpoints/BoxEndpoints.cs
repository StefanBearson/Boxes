using FluentValidation;
using magic_backend.Filters;
using magic_backend.Mapping;
using magic_backend.Models;
using magic_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace magic_backend.Endpoints;

public class BoxEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("").WithTags("Box");

        group.MapGet("/boxes", GetBoxes)
            .WithName("GetBoxes")
            .WithDescription("Get all magic boxes from the database");
        group.MapPost("/box", AddBox)
            .WithName("AddBox")
            .WithDescription("Add a new magic box to the database")
            .AddEndpointFilter<ValidationFilter<BoxVM>>()
            .ProducesValidationProblem();
        group.MapDelete("/boxes", RemoveAllBoxes)
            .WithName("RemoveAllBoxes")
            .WithDescription("Remove all magic boxes from the database");
    }
    
    private static async Task<IResult> AddBox([FromBody] BoxVM box, IBoxService boxService)
    {
        BoxDTO boxDto = box.ToDTO();
        boxService.AddBox(boxDto);

        return Results.Ok(box.ToDTO());
    }
    
    private static async Task<IResult> GetBoxes(IBoxService boxService)
    {
        var result = boxService.GetBoxes();

        return Results.Ok(result.Result.Select(box => box.ToVM()));
    }
    
    private static async Task<IResult> RemoveAllBoxes(IBoxService boxService)
    {
        boxService.RemoveAllBoxes();

        return Results.Ok("All boxes removed");
    }
    
    public class AddBoxValidator : AbstractValidator<BoxVM>
    {
        public AddBoxValidator()
        { 
            RuleFor(x => x.Color).NotEmpty();
            RuleFor(x => x.CurrentPositionY).NotNull().ExclusiveBetween(-1, 1001);
            RuleFor(x => x.CurrentPositionX).NotNull().ExclusiveBetween(-1, 1001);
            RuleFor(x => x.IsNewLayer).NotEmpty();
            RuleFor(x => x.Key).NotNull().ExclusiveBetween(-1, 10001);
            RuleFor(x => x.Row).NotNull().ExclusiveBetween(-1, 1001);
            RuleFor(x => x.X).NotNull().ExclusiveBetween(-1, 1001);
            RuleFor(x => x.Y).NotNull().ExclusiveBetween(-1, 1001);
        }
    }
}