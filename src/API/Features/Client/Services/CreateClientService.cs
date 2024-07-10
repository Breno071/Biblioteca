using API.Features.Client.Endpoints.CreateClient;
using Infraestructure.Data;

namespace API.Features.Client.Services
{
    public interface ICreateClientService
    {
        Task<CreateClientResponse> CreateClientAsync(CreateClientRequest request, CancellationToken ct);
    }

    public class CreateClientService(BaseDbContext _dbContext) : ICreateClientService
    {
        public async Task<CreateClientResponse> CreateClientAsync(CreateClientRequest request, CancellationToken ct)
        {
            var client = new Domain.Models.Entities.Client
            {
                ClientId = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
            };

            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync(ct);

            return new CreateClientResponse
            {
                ClientId = client.ClientId,
                Email = client.Email,
                Name = client.Name
            };
        }
    }
}
