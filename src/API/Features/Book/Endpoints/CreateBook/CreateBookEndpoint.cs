using API.Features.Book.Endpoints.GetBook;
using API.Features.Book.Services;
using API.Features.Client.Services;
using API.Shared.Extensions;
using Domain.Models.DTO;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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
            var livro = await Resolve<IGetBookService>().GetBooksByTitleAsync(req.Title, ct);

            if (livro is not null)
                return TypedResults.BadRequest();

            var result = await Resolve<ICreateBookService>().CreateBookAsync(req, ct);

            return TypedResults.Created(HttpContext.CreatedUri(result.Code), result);
        }
    }
}
