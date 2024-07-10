using API.Features.Book.Services;
using API.Features.Client.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.DeleteBook
{
    public class DeleteBookEndpoint : Endpoint<DeleteBookRequest, Results<NoContent, NotFound>>
    {
        public override void Configure()
        {
            Delete($"/web/book/{{{nameof(DeleteBookRequest.BookId)}}}");
            AllowAnonymous();
            Summary(b =>
            {
                b.Summary = "Deletes a book";
                b.Description = "Deletes a book";
                b.Responses[204] = "The book was deleted";
                b.Responses[404] = "The book was not found";
            });
        }

        public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteBookRequest req, CancellationToken ct)
        {
            var book = await Resolve<IGetBookService>().GetBookByIdAsync(req.BookId, ct);

            if (book == null)
                return TypedResults.NotFound();

            await Resolve<IDeleteBookService>().DeleteBookAsync(req.BookId, ct);
            return TypedResults.NoContent();
        }
    }
}
