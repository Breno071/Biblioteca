using Domain.Enums;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Core.Tokens;

namespace API.Features.Book.Services
{
    public interface IGetBookService
    {
        Task<Domain.Models.Entities.Book?> GetBookByIdAsync(Guid bookId, CancellationToken ct);
        Task<List<Domain.Models.Entities.Book>> GetBooksByTitleAsync(string title, CancellationToken ct);
        Task<List<Domain.Models.Entities.Book>> GetBooksByAuthorAsync(string author, CancellationToken ct);
        Task<List<Domain.Models.Entities.Book>> GetBooksByYearAsync(int year, CancellationToken ct);
        Task<List<Domain.Models.Entities.Book>> GetBooksByPublisherAsync(string publisher, CancellationToken ct);
        Task<List<Domain.Models.Entities.Book>> GetBooksByGenreAsync(Genre genre, CancellationToken ct);
        Task<List<Domain.Models.Entities.Book>> GetPaginatedBooksAsync(int page, int pageSize, CancellationToken ct);
    }

    public class GetBookService(BaseDbContext dbContext) : IGetBookService
    {
        public Task<List<Domain.Models.Entities.Book>> GetBooksByAuthorAsync(string author, CancellationToken ct)
        {
            return dbContext.Books.Where(b => b.Author == author && b.Active).ToListAsync(ct);
        }

        public Task<List<Domain.Models.Entities.Book>> GetBooksByGenreAsync(Genre genre, CancellationToken ct)
        {
            return dbContext.Books.Where(b => b.Genre == genre && b.Active).ToListAsync(ct);

        }

        public Task<Domain.Models.Entities.Book?> GetBookByIdAsync(Guid bookId, CancellationToken ct)
        {
            return dbContext.Books.SingleOrDefaultAsync(b => b.BookId == bookId && b.Active, ct);
        }

        public Task<List<Domain.Models.Entities.Book>> GetBooksByPublisherAsync(string publisher, CancellationToken ct)
        {
            return dbContext.Books.Where(b => b.Publisher == publisher && b.Active).ToListAsync(ct);
        }

        public Task<List<Domain.Models.Entities.Book>> GetBooksByTitleAsync(string title, CancellationToken ct)
        {
            return dbContext.Books.Where(b => b.Title == title && b.Active).ToListAsync(ct);
        }

        public Task<List<Domain.Models.Entities.Book>> GetBooksByYearAsync(int year, CancellationToken ct)
        {
            return dbContext.Books.Where(b => b.Year == year && b.Active).ToListAsync(ct);
        }

        public Task<List<Domain.Models.Entities.Book>> GetPaginatedBooksAsync(int page, int pageSize, CancellationToken ct)
        {
            return dbContext.Books.Skip(page).Take(pageSize).ToListAsync(ct);
        }
    }
}
