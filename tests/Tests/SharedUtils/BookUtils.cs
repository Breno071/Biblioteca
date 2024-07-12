using AutoFixture;
using Infraestructure.Data;

namespace Tests.SharedUtils
{
    public static class BookUtils
    {
        public static async Task<List<Domain.Models.Entities.Book>> AddBooksOnDb(this Fixture fixture, BaseDbContext dbContext, int qtd, bool active = true)
        {
            var books = fixture
                .Build<Domain.Models.Entities.Book>()
                .With(x => x.Active, active)
                .CreateMany(qtd)
                .ToList();

            dbContext.AddRange(books);
            await dbContext.SaveChangesAsync();

            return books;
        }
    }
}
