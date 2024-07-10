using API.Features.Book.DTOs;
using API.Features.Book.Services;
using API.Features.Client.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.GetBooksByGenre
{
    public class GetBookByGenreEndpoint : Endpoint<GetBookByGenreRequest, Results<Ok<List<BookDetailsDto>>, NotFound>>
    {
        public override void Configure()
        {
            Get($"/web/books/genre/{{{nameof(GetBookByGenreRequest.Genre)}}}");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Get a book by Genre";
                s.Responses[200] = "The book was found";
                s.Responses[404] = "The book was not found";
                s.Description = "Get a book by Genre";
            });
            Tags(BookFeature.Tags);
            Version(BookFeature.Version);
        }

        public override async Task<Results<Ok<List<BookDetailsDto>>, NotFound>> ExecuteAsync(GetBookByGenreRequest req, CancellationToken ct)
        {
            var result = await Resolve<IGetBookService>().GetBooksByGenreAsync(req.Genre, ct);

            return result is not null
                ? TypedResults.Ok(result.ConvertAll(x => new BookDetailsDto()
                {
                    BookId = x.BookId,
                    Title = x.Title,
                    Author = x.Author,
                    Genre = x.Genre,
                    Year = x.Year,
                    Stock = x.Stock,
                    Publisher = x.Publisher
                }))
                : TypedResults.NotFound();
        }
    }
}
