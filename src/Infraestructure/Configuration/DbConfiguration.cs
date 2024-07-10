using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Configuration
{
    public static class DbConfiguration
    {
        public static void AddLibraryDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BaseDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void InitializeMigration(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<BaseDbContext>();
            context.Database.Migrate();
        }
    }
}
