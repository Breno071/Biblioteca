using Infraestructure.Data;

namespace API.Features.Book.Services
{
    public interface IDeleteBookService
    {
        Task DeleteBookAsync(Guid code, CancellationToken ct);
    }

    public class DeleteBookService(BaseDbContext dbContext) : IDeleteBookService
    {
        public Task DeleteBookAsync(Guid code, CancellationToken ct)
        {
            var book = dbContext.Books.SingleOrDefault(b => b.Code == code && b.Active)!;
            book.Active = false;
            dbContext.Books.Update(book);
            dbContext.SaveChangesAsync(ct);
            return Task.CompletedTask;
        }
    }
}
