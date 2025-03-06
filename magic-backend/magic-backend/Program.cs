using magic_backend.Data;
using magic_backend.Mapping;
using magic_backend.Models;
using magic_backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IBoxService, BoxService>();

builder.Services.AddDbContext<BoxDbContext>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("BoxDb") ?? string.Empty));

builder.Services.AddCors(options =>
{
    options.AddPolicy("localDev", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new() { Title = "Magic API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.UseCors("localDev");
}

app.UseHttpsRedirection();



app.MapPost("/box", (BoxVM box, IBoxService boxService) =>
    {
        BoxDTO boxDto = box.ToDTO();
        var result = boxService.AddBox(boxDto);
        
        return Results.Ok(box.ToDTO());
    })
    .WithName("AddBox")
    .WithDescription("Add a new magic box to the database");

app.MapGet("/box", (IBoxService boxService) =>
    {
        var result = boxService.GetBoxes();
        
        return Results.Ok(result.Result.Select(box => box.ToVM()));
    })
    .WithName("GetAllBoxes")
    .WithDescription("Get all magic boxes from the database");

app.MapDelete("/box", (IBoxService boxService) =>
    {
        boxService.RemoveAllBoxes();
        
        return Results.Ok("All boxes removed");
    })
    .WithName("RemoveAllBoxes")
    .WithDescription("Remove all magic boxes from the database");


app.Run();

