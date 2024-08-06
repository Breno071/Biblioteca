using API.Shared.Extensions;
using Core.Configuration;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infraestructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors();
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Biblioteca";
            s.Version = "v1";
            s.Description = "API para o gerenciamento de biblioteca";
        };
    });

//Configurations
builder.Services
    .AddInfraestructure()
    .AddApplicationCore();

// Inject logging
builder.Services.ConfigureSerilog();

builder.Services.RegisterFeatures();

var app = builder.Build();

//Initialize Migrations
app.Services.InitializeMigration();

app.UseCors(x => x
    .SetIsOriginAllowed(options => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);

app.UseFastEndpoints()
    .UseSwaggerGen();

await app.RunAsync();

public partial class Program { }