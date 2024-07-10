using API.Features.Book.DTOs;
using API.Features.Book.Endpoints.GetBook;
using API.Features.Book.Services;
using API.Features.Client.Services;
using Domain.Models.DTO;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.GetBookById
{
    public class GetBookByIdEndpoint : Endpoint<GetBookByIdRequest, Results<Ok<BookDetailsDto>, NotFound>>
    {
        public override void Configure()
        {
            Get($"/web/book/code/{{{nameof(GetBookByIdRequest.Code)}}}");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Get a book by code";
                s.Responses[200] = "The book was found";
                s.Responses[404] = "The book was not found";
                s.Description = "Get a book by code";
            });
            Tags(BookFeature.Tags);
            Version(BookFeature.Version);
        }

        public override async Task<Results<Ok<BookDetailsDto>, NotFound>> ExecuteAsync(GetBookByIdRequest req, CancellationToken ct)
        {
            var result = await Resolve<IGetBookService>().GetBookByIdAsync(req.Code, ct);

            return result is not null
                ? TypedResults.Ok(new BookDetailsDto()
                {
                    Code = result.Code,
                    Title = result.Title,
                    Author = result.Author,
                    Genre = result.Genre,
                    Year = result.Year,
                    Stock = result.Stock,
                    Publisher = result.Publisher
                })
                : TypedResults.NotFound();
        }
    }
}
