using API.Features.Book.DTOs;
using API.Features.Book.Services;
using API.Features.Client.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.GetBooksByAuthor
{
    public class GetBookByAuthorEndpoint : Endpoint<GetBookByAuthorRequest, Results<Ok<List<BookDetailsDto>>, NotFound>>
    {
        public override void Configure()
        {
            Get($"/web/books/author/{{{nameof(GetBookByAuthorRequest.Author)}}}");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Get a book by Author";
                s.Responses[200] = "The book was found";
                s.Responses[404] = "The book was not found";
                s.Description = "Get a book by Author";
            });
            Tags(BookFeature.Tags);
            Version(BookFeature.Version);
        }

        public override async Task<Results<Ok<List<BookDetailsDto>>, NotFound>> ExecuteAsync(GetBookByAuthorRequest req, CancellationToken ct)
        {
            var result = await Resolve<IGetBookService>().GetBooksByAuthorAsync(req.Author, ct);

            return result is not null
                ? TypedResults.Ok(result.ConvertAll(x => new BookDetailsDto()
                {
                    Code = x.Code,
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
