using FluentValidation;
using magic_backend.Filters;
using magic_backend.Mapping;
using magic_backend.Models;
using magic_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace magic_backend.Endpoints;

public class BoxEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("").WithTags("Box");

        group.MapGet("/box", GetBoxes)
            .WithName("GetBoxes")
            .WithDescription("Get all boxes from the database.")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all boxes",
                Description = "This will return all the saved boxes from the database.",
                Responses =
                {
                    ["200"] = new OpenApiResponse { Description = "Greeting the stored boxes returned successfully." },
                    ["400"] = new OpenApiResponse { Description = "The request is invalid." }
                }
            });
        group.MapPost("/box", CreateBox)
            .WithName("AddBox")
            .WithDescription("Add a new box to the database.")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new box",
                Description = "This will add a new box to the database.",
                RequestBody = new OpenApiRequestBody
                {
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "BoxVM" }
                            }
                        }
                    }
                },
                Responses =
                {
                    ["200"] = new OpenApiResponse { Description = "Add box returned successfully." },
                    ["400"] = new OpenApiResponse { Description = "The request is invalid." },
                    ["500"] = new OpenApiResponse { Description = "An unexpected error occurred." }
                }
            })
            .AddEndpointFilter<ValidationFilter<BoxVM>>()
            .ProducesValidationProblem();
        group.MapDelete("/box", RemoveAllBoxes)
            .WithName("RemoveAllBoxes")
            .WithDescription("Remove all boxes from the database.").WithOpenApi(operation => new(operation)
            {
                Summary = "Remove all the boxes",
                Description = "This will remove all the saved boxes from the database. This is not revocable",
                Responses =
                {
                    ["200"] = new OpenApiResponse { Description = "Removes all the boxes returned successfully." },
                    ["400"] = new OpenApiResponse { Description = "The request is invalid." }
                }
            });
    }
    
    public static async Task<IResult> CreateBox([FromBody] BoxVM box, IBoxService boxService)
    {
        var boxDto = box.ToDTO();
        await boxService.CreateBox(boxDto);
        return Results.Ok(boxDto);
    }
    
    public static async Task<IResult> GetBoxes(IBoxService boxService)
    {
        var result = boxService.GetBoxes();
        return Results.Ok(result.Result.Select(box => box.ToVM()));
    }
    
    public static async Task<IResult> RemoveAllBoxes(IBoxService boxService)
    {
        await boxService.RemoveAllBoxes();
        return Results.Ok("All boxes removed");
    }
    
    public class AddBoxValidator : AbstractValidator<BoxVM>
    {
        public AddBoxValidator()
        { 
            RuleFor(x => x.Color).NotEmpty();
            RuleFor(x => x.IsNewLayer).NotEmpty();
            RuleFor(x => x.Key).NotNull().ExclusiveBetween(-1, 10001);
            RuleFor(x => x.Row).NotNull().ExclusiveBetween(-1, 1001);
            RuleFor(x => x.X).NotNull().ExclusiveBetween(-1, 1001);
            RuleFor(x => x.Y).NotNull().ExclusiveBetween(-1, 1001);
        }
    }
}