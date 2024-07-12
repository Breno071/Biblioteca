using AutoFixture;
using Infraestructure.Data;

namespace Tests.SharedUtils
{
    public static class ClientUtils
    {
        public static async Task<List<Domain.Models.Entities.Client>> AddClientsToDb(this Fixture fixture, BaseDbContext dbContext, int qtd)
        {
            var clients = fixture
                .Build<Domain.Models.Entities.Client>()
                .CreateMany(qtd)
                .ToList();

            dbContext.AddRange(clients);
            await dbContext.SaveChangesAsync();

            return clients;
        }
    }
}
