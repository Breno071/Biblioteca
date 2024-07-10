using FluentValidation;
using FluentValidation.Validators;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Book.Validators
{
    public class BookShouldHaveStock<T> : IAsyncPropertyValidator<T, Guid> where T : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BookShouldHaveStock(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public string Name => "Stock";

        public string GetDefaultMessageTemplate(string errorCode) => "Book without stock";

        public async Task<bool> IsValidAsync(ValidationContext<T> context, Guid value, CancellationToken cancellation)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var book = await dbContext.Books
                .AsNoTracking()
                .SingleOrDefaultAsync(b =>
                    b.Code == value &&
                    b.Active, cancellationToken: cancellation);

            //Se o livro não existir ele cai em outro validator(BookShouldExistValidator)
            if (book is null)
                return true;

            return book.Stock > 0;
        }
    }
}
