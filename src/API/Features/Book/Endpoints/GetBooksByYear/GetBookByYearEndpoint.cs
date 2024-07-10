using API.Features.Book.DTOs;
using API.Features.Book.Services;
using API.Features.Client.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.GetBooksByYear
{
    public class GetBookByYearEndpoint : Endpoint<GetBookByYearRequest, Results<Ok<List<BookDetailsDto>>, NotFound>>
    {
        public override void Configure()
        {
            Get($"/web/books/year/{{{nameof(GetBookByYearRequest.Year)}}}");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Get a book by Year";
                s.Responses[200] = "The book was found";
                s.Responses[404] = "The book was not found";
                s.Description = "Get a book by Year";
            });
            Tags(BookFeature.Tags);
            Version(BookFeature.Version);
        }

        public override async Task<Results<Ok<List<BookDetailsDto>>, NotFound>> ExecuteAsync(GetBookByYearRequest req, CancellationToken ct)
        {
            var result = await Resolve<IGetBookService>().GetBooksByYearAsync(req.Year, ct);

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
