using API.Shared.Extensions;
using Domain.Interfaces;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infraestructure.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Producer;
using Serilog;
using Serilog.Sinks.MSSqlServer;

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
builder.Services.AddLibraryDbContext(builder.Configuration);

builder.Services.AddScoped<ConnectionFactory>();
builder.Services.AddScoped<IProducer, Producer>();

// Inject logging
builder.Services.AddLogging(options =>
{
    var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "TB_LOG", AutoCreateSqlTable = true }
                )
                .CreateLogger();

    options.AddSerilog(logger);
});

builder.Services.RegisterFeatures();

var app = builder.Build();

//Initialize Migrations
DbConfiguration.InitializeMigration(app.Services);


app.UseCors(x => x
    .SetIsOriginAllowed(options => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);

app.UseFastEndpoints().UseSwaggerGen();

await app.RunAsync();

public partial class Program { }