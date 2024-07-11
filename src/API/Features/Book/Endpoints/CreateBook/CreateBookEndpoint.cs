using API.Features.Book.Services;
using API.Shared.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Book.Endpoints.CreateBook
{
    public class CreateBookEndpoint : Endpoint<CreateBookRequest, Results<Created<CreateBookResponse>, BadRequest>>
    {

        public override void Configure()
        {
            Post("/web/book");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Create a new book";
                s.Description = "Create a new book";
            });
            Tags(BookFeature.Tags);
            Version(BookFeature.Version);
        }

        public override async Task<Results<Created<CreateBookResponse>, BadRequest>> ExecuteAsync(CreateBookRequest req, CancellationToken ct)
        {
            var result = await Resolve<ICreateBookService>().CreateBookAsync(req, ct);

            return TypedResults.Created(HttpContext.CreatedUri(result.BookId), result);
        }
    }
}
