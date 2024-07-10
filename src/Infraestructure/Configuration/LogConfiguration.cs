using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Infraestructure.Configuration
{
    public static class LogConfiguration
    {
        public static IServiceCollection ConfigureSerilog(this IServiceCollection services)
        {
            var currentyDirectory = Directory.GetCurrentDirectory();
            var parentDirectory = Directory.GetParent(currentyDirectory)!;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(parentDirectory.FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddLogging(options =>
            {
                var logger = new LoggerConfiguration()
                            .MinimumLevel.Information()
                            .WriteTo.MSSqlServer(
                                connectionString: configuration.GetConnectionString("DefaultConnection"),
                                sinkOptions: new MSSqlServerSinkOptions { TableName = "Log", AutoCreateSqlTable = true }
                            )
                            .CreateLogger();

                options.AddSerilog(logger);
            });

            return services;
        }
    }
}
