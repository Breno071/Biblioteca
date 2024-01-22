using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data
{
    public class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
