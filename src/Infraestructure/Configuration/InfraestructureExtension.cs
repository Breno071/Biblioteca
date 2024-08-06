using Infraestructure.Data;
using Infraestructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Configuration
{
    public static class InfraestructureExtension
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services)
        {
            var solutionFolder = DirectoryHelper.FindSolutionDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(solutionFolder.FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddDbContext<BaseDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }        

        public static IServiceProvider InitializeMigration(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<BaseDbContext>();
            context.Database.Migrate();

            return serviceProvider;
        }
    }
}
