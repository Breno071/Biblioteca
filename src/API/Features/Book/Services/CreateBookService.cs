using API.Features.Book.Endpoints.CreateBook;
using Infraestructure.Data;

namespace API.Features.Book.Services
{
    public interface ICreateBookService
    {
        Task<CreateBookResponse> CreateBookAsync(CreateBookRequest request, CancellationToken ct);
    }

    public class CreateBookService(BaseDbContext _dbContext) : ICreateBookService
    {
        public async Task<CreateBookResponse> CreateBookAsync(CreateBookRequest request, CancellationToken ct)
        {
            var book = new Domain.Models.Entities.Book
            {
                Code = Guid.NewGuid(),
                Title = request.Title,
                Author = request.Author,
                Year = request.Year,
                Publisher = request.Publisher,
                Genre = request.Genre
            };           

            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync(ct);

            return new CreateBookResponse
            {
                Code = book.Code,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Publisher = book.Publisher,
                Genre = book.Genre
            };
        }
    }
}
