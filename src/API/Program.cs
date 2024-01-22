using Serilog.Sinks.MSSqlServer;
using Serilog;
using Infraestructure.Configuration;
using ApplicationCore.Configuration.Mappers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddCors();

//Configurations
DbConfiguration.AddDbContext(builder.Services, builder.Configuration);
AutoMapperConfigurationBuilder.ConfigureAutoMapper(builder.Services);

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

var app = builder.Build();

//Initialize Migrations
DbConfiguration.InitializeMigration(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .SetIsOriginAllowed(options => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);

app.MapControllers();

app.Run();

