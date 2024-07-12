using Infraestructure.Data;

namespace API.Features.Book.Services
{
    public interface IDeleteBookService
    {
        Task DeleteBookAsync(Guid bookId, CancellationToken ct);
    }

    public class DeleteBookService(BaseDbContext dbContext) : IDeleteBookService
    {
        public Task DeleteBookAsync(Guid bookId, CancellationToken ct)
        {
            var book = dbContext.Books.SingleOrDefault(b => b.BookId == bookId && b.Active)!;
            book.Active = false;
            dbContext.Books.Update(book);
            dbContext.SaveChangesAsync(ct);
            return Task.CompletedTask;
        }
    }
}
