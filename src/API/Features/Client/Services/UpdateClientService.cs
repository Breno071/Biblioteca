using API.Features.Client.DTOs;
using API.Features.Client.Endpoints.UpdateClient;
using Infraestructure.Data;

namespace API.Features.Client.Services
{
    public interface IUpdateClientService
    {
        Task<ClientDetailsDto> UpdateClientAsync(UpdateClientRequest request, CancellationToken ct);
    }

    public class UpdateClientService(BaseDbContext dbContext) : IUpdateClientService
    {
        public Task<ClientDetailsDto> UpdateClientAsync(UpdateClientRequest request, CancellationToken ct)
        {
            var Client = dbContext.Clients.SingleOrDefault(b => b.Code == request.Code)!;

            Client.Email = request.Email;
            Client.Name = request.Name;

            dbContext.Clients.Update(Client);
            dbContext.SaveChangesAsync(ct);

            return Task.FromResult(new ClientDetailsDto
            {
                Code = Client.Code,
                Email = Client.Email,
                Name = Client.Name
            });
        }
    }
}
