using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureEntity<TEntity, TRelatedEntity, TJoinEntity>(this ModelBuilder modelBuilder) 
            where TEntity : class
            where TRelatedEntity : class
            where TJoinEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .HasMany<TRelatedEntity>()
                .WithMany()
                .UsingEntity<TJoinEntity>();
        }
    }
}
