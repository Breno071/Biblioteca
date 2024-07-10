using API.Features.Book.DTOs;
using API.Features.Book.Services;
using API.Features.Client.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.GetBooksPaginated
{
    public class GetBooksPaginatedEndpoint : Endpoint<GetBooksPaginatedRequest, Results<Ok<List<BookDetailsDto>>, BadRequest>>
    {
        public override void Configure()
        {
            Get($"/web/books");
            AllowAnonymous();
            Summary(x => 
            {
                x.Summary = "Get paginated books";                
            });
            Tags(BookFeature.Tags);
            Version(BookFeature.Version);
        }

        public override async Task<Results<Ok<List<BookDetailsDto>>, BadRequest>> ExecuteAsync(GetBooksPaginatedRequest req, CancellationToken ct)
        {
            if(req.PageSize > 1000)
                return TypedResults.BadRequest();
               
            var result = await Resolve<IGetBookService>().GetPaginatedBooksAsync(req.Page, req.PageSize, ct);

            return TypedResults.Ok(result.ConvertAll(b => new BookDetailsDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Year = b.Year,
                Author = b.Author,
                Publisher = b.Publisher,
                Genre = b.Genre,
                Stock = b.Stock
            }));
        }
    }
}
