using API.Features.Client.DTOs;
using API.Features.Client.Services;
using API.Shared.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Client.Endpoints.GetClientById
{
    public class GetClienteByIdEndpoint : Endpoint<GetClientByIdRequest, Results<Ok<ClientDetailsDto>, NotFound>>
    {
        public override void Configure()
        {
            Get($"/web/client/code/{{{nameof(GetClientByIdRequest.Code)}}}");
            AllowAnonymous();
            Summary(x =>
            {
                x.Summary = "Get a client by id";
                x.ResponseExamples[200] = new ClientDetailsDto();
                x.Responses[200] = "Client found";
                x.Responses[404] = "Client not found";
            });
            Tags(ClientFeature.Tags);
            Version(ClientFeature.Version);
        }

        public override async Task<Results<Ok<ClientDetailsDto>, NotFound>> ExecuteAsync(GetClientByIdRequest req, CancellationToken ct)
        {
            var client = await Resolve<IGetClientService>().GetClientByIdAsync(req.Code, ct);

            if (client is null)
                return TypedResults.NotFound();

            var response = new ClientDetailsDto
            {
                Code = client.Code,
                Email = client.Email,
                Name = client.Name
            };

            return TypedResults.Ok(response);
        }
    }
}
