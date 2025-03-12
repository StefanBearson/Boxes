using FluentValidation;
using magic_backend.Filters;
using magic_backend.Mapping;
using magic_backend.Models;
using magic_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace magic_backend.Endpoints;

public static class BoxEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("").WithTags("Box");

        group.MapGet("/box", GetBoxes)
            .WithName("GetBoxes")
            .WithDescription("GET endpoint for retrieving all boxes from the database.")
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
            .WithDescription("POST endpoint for creating a new box in the database.")
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
                    ["201"] = new OpenApiResponse { Description = "Add box successfully." },
                    ["400"] = new OpenApiResponse { Description = "The request is invalid." },
                    ["500"] = new OpenApiResponse { Description = "An unexpected error occurred." }
                }
            })
            .AddEndpointFilter<ValidationFilter<BoxVM>>()
            .ProducesValidationProblem();
        group.MapDelete("/box", DeleteAllBoxes)
            .WithName("DeleteAllBoxes")
            .WithDescription("DELETE endpoint for removing all boxes from the database.").WithOpenApi(operation => new(operation)
            {
                Summary = "Delete all the boxes",
                Description = "This will delete all the saved boxes from the database. This is not revocable",
                Responses =
                {
                    ["200"] = new OpenApiResponse { Description = "Delete all the boxes returned successfully." },
                    ["400"] = new OpenApiResponse { Description = "The request is invalid." }
                }
            });
    }
    
    internal static async Task<IResult> CreateBox([FromBody] BoxVM box, IBoxService boxService)
    {
        var boxDto = box.ToDTO();
        await boxService.CreateBox(boxDto);
        return Results.Created("", boxDto);
    }
    
    internal static async Task<IResult> GetBoxes(IBoxService boxService)
    {
        var result = await boxService.GetBoxes();
        return Results.Ok(result.Select(box => box.ToVM()));
    }
    
    internal static async Task<IResult> DeleteAllBoxes(IBoxService boxService)
    {
        var message = await boxService.DeleteAllBoxes();
        return Results.Ok(message);
    }
    
    public class AddBoxValidator : AbstractValidator<BoxVM>
    {
        // Ensures that the Color property is not empty, has a maximum length of 7 characters,
        // has a minimum length of 4 characters,
        // and matches the regular expression pattern allowing only alphanumeric characters and '#'.
        // This Sanitizes the input.
        // Ensures that the Row, X, and Y properties are not null and are greater than or equal to 0.
        // Ensures that the Key, Row, X, and Y property is not null and is greater than or equal to 0.
        public AddBoxValidator()
        { 
            RuleFor(x => x.Color).NotEmpty().MinimumLength(4).MaximumLength(7).Matches("^[a-zA-Z0-9#]*$");
            RuleFor(x => x.IsNewLayer);
            RuleFor(x => x.Key).NotNull().ExclusiveBetween(-1, Int32.MaxValue);
            RuleFor(x => x.Row).NotNull().ExclusiveBetween(-1, Int32.MaxValue);
            RuleFor(x => x.X).NotNull().ExclusiveBetween(-1, Int32.MaxValue);
            RuleFor(x => x.Y).NotNull().ExclusiveBetween(-1, Int32.MaxValue);
        }
    }
}