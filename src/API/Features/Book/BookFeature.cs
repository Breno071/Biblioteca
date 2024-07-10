using API.Features.Book.Services;
using API.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API.Features.Book
{
    public class BookFeature : IFeature
    {
        public static readonly string[] Tags = ["Books"];
        public const int Version = 0;

        public IServiceCollection RegisterFeature(IServiceCollection services)
        {
            services.TryAddScoped<IDeleteBookService, DeleteBookService>();
            services.TryAddScoped<IUpdateBookService, UpdateBookService>();
            services.TryAddScoped<ICreateBookService, CreateBookService>();
            services.TryAddScoped<IGetBookService, GetBookService>();

            return services;
        }
    }
}
