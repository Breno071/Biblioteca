using FluentValidation;
using FluentValidation.Validators;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Book.Validators
{
    public class BookShouldExistValidator<T> : IAsyncPropertyValidator<T, Guid> where T : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BookShouldExistValidator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public string Name => "BookId";

        public string GetDefaultMessageTemplate(string errorCode) => "Book not found!";

        public async Task<bool> IsValidAsync(ValidationContext<T> context, Guid value, CancellationToken cancellation)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var bookExist = await dbContext.Books
                .AsNoTracking()
                .AnyAsync(b => b.BookId == value && b.Active, cancellationToken: cancellation);

            return !bookExist;
        }
    }
}
