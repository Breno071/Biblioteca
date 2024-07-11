using API.Features.Client.DTOs;
using API.Features.Client.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Client.Endpoints.UpdateClient
{
    public class UpdateClientEndpoint : Endpoint<UpdateClientRequest, Results<Ok<ClientDetailsDto>, NotFound>>
    {
        public override void Configure()
        {
            Put($"/web/client/{{{nameof(UpdateClientRequest.ClientId)}}}");
            AllowAnonymous();
            Summary(x =>
            {
                x.Summary = "Update a client";
                x.ResponseExamples[200] = new ClientDetailsDto();
                x.Responses[200] = "Client updated";
                x.Responses[404] = "Client not found";
            });
            Tags(ClientFeature.Tags);
            Version(ClientFeature.Version);
        }

        public override async Task<Results<Ok<ClientDetailsDto>, NotFound>> ExecuteAsync(UpdateClientRequest req, CancellationToken ct)
        {
            var client = await Resolve<IGetClientService>().GetClientByIdAsync(req.ClientId, ct);

            if (client is null)
                return TypedResults.NotFound();

            ThrowIfAnyErrors();

            var response = await Resolve<IUpdateClientService>().UpdateClientAsync(req, ct);

            return TypedResults.Ok(response);
        }
    }
}
