using AutoFixture;
using Infraestructure.Data;

namespace Tests.SharedUtils
{
    public static class BookUtils
    {
        public static async Task<List<Domain.Models.Entities.Book>> AddBooksOnDb(this Fixture fixture, BaseDbContext dbContext, int qtd)
        {
            var books = fixture
                .Build<Domain.Models.Entities.Book>()
                .CreateMany(qtd)
                .ToList();

            dbContext.AddRange(books);
            await dbContext.SaveChangesAsync();

            return books;
        }
    }
}
