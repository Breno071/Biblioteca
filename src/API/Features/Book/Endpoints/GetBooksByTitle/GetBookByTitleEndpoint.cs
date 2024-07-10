﻿using API.Features.Book.DTOs;
using API.Features.Book.Services;
using API.Features.Client.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.GetBooksByTitle
{
    public class GetBookByTitleEndpoint : Endpoint<GetBookByTitleRequest, Results<Ok<List<BookDetailsDto>>, NotFound>>
    {
        public override void Configure()
        {
            Get($"/web/books/title/{{{nameof(GetBookByTitleRequest.Title)}}}");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Get a book by Title";
                s.Responses[200] = "The book was found";
                s.Responses[404] = "The book was not found";
                s.Description = "Get a book by Title";
            });
            Tags(BookFeature.Tags);
            Version(BookFeature.Version);
        }

        public override async Task<Results<Ok<List<BookDetailsDto>>, NotFound>> ExecuteAsync(GetBookByTitleRequest req, CancellationToken ct)
        {
            var result = await Resolve<IGetBookService>().GetBooksByTitleAsync(req.Title, ct);

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
