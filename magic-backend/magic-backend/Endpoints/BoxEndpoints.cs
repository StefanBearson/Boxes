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
        group.MapPost("/box", AddBox)
            .WithName("AddBox")
            .WithDescription("Add a new box to the database.")
            // .WithOpenApi(operation => new(operation)
            // {
            //     Summary = "Create a new box",
            //     Description = "This will add a new box to the database.",
            //     Parameters =
            //     [
            //         new()
            //         {
            //             Name = "key",
            //             Description = "Unique identifier for the box.",
            //             Required = true,
            //             Schema = new OpenApiSchema
            //             {
            //                 Type = "integer",
            //             }
            //         },
            //         new()
            //         {
            //             Name = "x",
            //             Description = "The x position of the box.",
            //             Required = true,
            //             Schema = new OpenApiSchema
            //             {
            //                 Type = "integer",
            //             }
            //         },
            //         new()
            //         {
            //             Name = "y",
            //             Description = "The y position of the box.",
            //             Required = true,
            //             Schema = new OpenApiSchema
            //             {
            //                 Type = "integer",
            //             }
            //         },
            //         new()
            //         {
            //             Name = "color",
            //             Description = "The Color of the box. in hex format. (e.g. #000000)",
            //             Required = true,
            //             Schema = new OpenApiSchema
            //             {
            //                 Type = "string",
            //             }
            //         },
            //         new()
            //         {
            //             Name = "row",
            //             Description = "The row in the the grid for the box. Important for the algorithm in the frontend.",
            //             Required = true,
            //             Schema = new OpenApiSchema
            //             {
            //                 Type = "integer",
            //             }
            //         },
            //         new()
            //         {
            //             Name = "isNewLayer",
            //             Description = "Is the box on a new layer. This is for the algorithm in the frontend to know if the box is on a new layer or not.",
            //             Required = true,
            //             Schema = new OpenApiSchema
            //             {
            //                 Type = "boolean",
            //             }
            //         },
            //         new()
            //         {
            //             Name = "currentPositionX",
            //             Description = "The current x position of the box. This is for the algorithm in the frontend to know the current position of the box.",
            //             Required = true,
            //             
            //             Schema = new OpenApiSchema
            //             {
            //                 Type = "integer",
            //             }
            //         },
            //         new()
            //         {
            //             Name = "currentPositionY",
            //             Description = "The current y position of the box. This is for the algorithm in the frontend to know the current position of the box.",
            //             Required = true,
            //             Schema = new OpenApiSchema
            //             {
            //                 Type = "integer",
            //             }
            //         },
            //     ],
            //     Responses =
            //     {
            //         ["200"] = new OpenApiResponse { Description = "Add box returned successfully." },
            //         ["400"] = new OpenApiResponse { Description = "The request is invalid." },
            //         ["500"] = new OpenApiResponse { Description = "An unexpected error occurred." }
            //     }
            // })
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
    
private static async Task<IResult> AddBox([FromBody] BoxVM box, IBoxService boxService)
    {
        var boxDto = box.ToDTO();
        await boxService.AddBox(boxDto);
        return Results.Ok(boxDto);
    }
    
    private static async Task<IResult> GetBoxes(IBoxService boxService)
    {
        var result = boxService.GetBoxes();
        return Results.Ok(result.Result.Select(box => box.ToVM()));
    }
    
    private static async Task<IResult> RemoveAllBoxes(IBoxService boxService)
    {
        await boxService.RemoveAllBoxes();
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