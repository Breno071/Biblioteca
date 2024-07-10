using API.Features.Client.Services;
using API.Shared.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Client.Endpoints.CreateClient
{
    public class CreateClientEndpoint : Endpoint<CreateClientRequest, Created<CreateClientResponse>>
    {
        public override void Configure()
        {
            Post($"/web/client");
            AllowAnonymous();
            Summary(x =>
            {
                x.Summary = "Creates a client";
                x.ResponseExamples[201] = new CreateClientResponse();
            });
            Tags(ClientFeature.Tags);
            Version(ClientFeature.Version);
            DontThrowIfValidationFails();
        }

        public override async Task<Created<CreateClientResponse>> ExecuteAsync(CreateClientRequest req, CancellationToken ct)
        {
            var result = await Resolve<ICreateClientService>().CreateClientAsync(req, ct);

            return TypedResults.Created(HttpContext.CreatedUri(result.ClientId), result);
        }
    }
}
