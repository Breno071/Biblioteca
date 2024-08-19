using Domain.Models.Entities;
using Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infraestructure.Data
{
    public class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureEntity<Reservation, Book, ReservationBook>();
            modelBuilder.ConfigureEntity<Book, Reservation, ReservationBook>();
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void RegisterAllEntities<TFeatureModel>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetExportedTypes()).Where(c => c is { IsClass: true, IsAbstract: false, IsPublic: true } && typeof(TFeatureModel).IsAssignableFrom(c));

            foreach (var type in types)
                modelBuilder.Entity(type);
        }
    }
}
