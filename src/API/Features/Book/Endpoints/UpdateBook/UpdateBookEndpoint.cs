using API.Features.Book.DTOs;
using API.Features.Book.Services;
using API.Features.Client.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.UpdateBook
{
    public class UpdateBookEndpoint : Endpoint<UpdateBookRequest, Results<Ok<BookDetailsDto>, NotFound>>
    {
        public override void Configure()
        {
            Put($"/web/book/{{{nameof(UpdateBookRequest.Code)}}}");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Update a book";
                s.Description = "Update a book";
                s.Response<Ok<BookDetailsDto>>(200, "Book updated successfully");
            });
            Tags(BookFeature.Tags);
            Version(BookFeature.Version);
        }

        public override async Task<Results<Ok<BookDetailsDto>, NotFound>> ExecuteAsync(UpdateBookRequest req, CancellationToken ct)
        {
            var book = await Resolve<IGetBookService>().GetBookByIdAsync(req.Code, ct);

            if (book is null)
                return TypedResults.NotFound();

            ThrowIfAnyErrors();

            var result = await Resolve<IUpdateBookService>().UpdateBookAsync(req, ct)!;

            return TypedResults.Ok(result);
        }
    }
}
