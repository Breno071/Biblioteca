using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Client.Services
{
    public interface IGetClientService
    {
        Task<Domain.Models.Entities.Client?> GetClientByIdAsync(Guid clientId, CancellationToken ct);
        Task<Domain.Models.Entities.Client?> GetClientsByEmailAsync(string email, CancellationToken ct);
    }

    public class GetClientService(BaseDbContext dbContext) : IGetClientService
    {

        public Task<Domain.Models.Entities.Client?> GetClientByIdAsync(Guid clientId, CancellationToken ct)
        {
            return dbContext.Clients.SingleOrDefaultAsync(b => b.ClientId == clientId, ct);
        }

        public Task<Domain.Models.Entities.Client?> GetClientsByEmailAsync(string email, CancellationToken ct)
        {
            return dbContext.Clients.SingleOrDefaultAsync(b => b.Email == email, ct);
        }
    }
}
