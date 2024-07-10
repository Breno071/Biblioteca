using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Client.Services
{
    public interface IGetClientService
    {
        Task<Domain.Models.Entities.Client?> GetClientByIdAsync(Guid code, CancellationToken ct);
        Task<Domain.Models.Entities.Client?> GetClientsByEmailAsync(string email, CancellationToken ct);
    }

    public class GetClientService(BaseDbContext dbContext) : IGetClientService
    {

        public Task<Domain.Models.Entities.Client?> GetClientByIdAsync(Guid code, CancellationToken ct)
        {
            return dbContext.Clients.SingleOrDefaultAsync(b => b.Code == code, ct);
        }

        public Task<Domain.Models.Entities.Client?> GetClientsByEmailAsync(string email, CancellationToken ct)
        {
            return dbContext.Clients.SingleOrDefaultAsync(b => b.Email == email, ct);
        }
    }
}
