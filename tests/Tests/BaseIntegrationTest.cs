using Domain.Models.Entities;
using Infraestructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebApiFactory>, IDisposable
    { 
        protected readonly IServiceScope _scope;
        protected readonly BaseDbContext DbContext;
        protected readonly HttpClient AnonymousUser;

        public BaseIntegrationTest(IntegrationTestWebApiFactory factory) 
        {
            _scope = factory.Services.CreateScope();
            DbContext = _scope.ServiceProvider.GetRequiredService<BaseDbContext>();
            AnonymousUser = factory.CreateClient();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            DbContext?.Dispose();
        }
    }
}
