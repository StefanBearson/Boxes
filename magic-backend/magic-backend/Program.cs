using FluentValidation;
using magic_backend.Data;
using magic_backend.Endpoints;
using magic_backend.ExceptionHandler;
using magic_backend.Middlewares;
using magic_backend.Repositorys;
using magic_backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IBoxService, BoxService>();

if (builder.Configuration.GetValue<bool>("UseJsonAsDatabase"))
{
    builder.Services.AddScoped<IBoxRepository, BoxJsonRepository>();
}
else
{
    builder.Services.AddScoped<IBoxRepository, BoxEFRepository>();
    builder.Services.AddDbContext<BoxDbContext>(options =>
        options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("BoxDb") ?? string.Empty));
}

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseHttpsRedirection();

BoxEndpoints.Map(app);

app.Run();