using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infraestructure.Configuration
{
    public class LibraryContextFactory : IDesignTimeDbContextFactory<BaseDbContext>
    {
        public BaseDbContext CreateDbContext(string[] args)
        {
            var currentyDirectory = Directory.GetCurrentDirectory();
            var parentDirectory = Directory.GetParent(currentyDirectory)!;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(parentDirectory.FullName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new BaseDbContext(optionsBuilder.Options);
        }
    }
}
