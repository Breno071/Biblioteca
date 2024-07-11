using Infraestructure.Data;
using Infraestructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infraestructure.Configuration
{
    public class LibraryContextFactory : IDesignTimeDbContextFactory<BaseDbContext>
    {
        public BaseDbContext CreateDbContext(string[] args)
        {
            var solutionFolder = DirectoryHelper.FindSolutionDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(solutionFolder.FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new BaseDbContext(optionsBuilder.Options);
        }
    }
}
