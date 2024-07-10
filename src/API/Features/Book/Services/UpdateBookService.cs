using API.Features.Book.DTOs;
using API.Features.Book.Endpoints.UpdateBook;
using Infraestructure.Data;

namespace API.Features.Book.Services
{
    public interface IUpdateBookService
    {
        Task<BookDetailsDto> UpdateBookAsync(UpdateBookRequest request, CancellationToken ct);
    }

    public class UpdateBookService(BaseDbContext dbContext) : IUpdateBookService
    {
        public Task<BookDetailsDto> UpdateBookAsync(UpdateBookRequest request, CancellationToken ct)
        {
            var book = dbContext.Books.SingleOrDefault(b => b.Code == request.Code && b.Active)!;

            book.Title = request.Title;
            book.Author = request.Author;
            book.Year = request.Year;
            book.Publisher = request.Publisher;
            book.Genre = request.Genre;
            book.Stock = request.Stock;

            dbContext.Books.Update(book);
            dbContext.SaveChangesAsync(ct);

            return Task.FromResult(new BookDetailsDto
            {
                Code = book.Code,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Publisher = book.Publisher,
                Genre = book.Genre,
                Stock = book.Stock
            });
        }
    }
}
