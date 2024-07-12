using API.Features.Client.Services;
using API.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API.Features.Client
{
    public class ClientFeature : IFeature
    {
        public static readonly string[] Tags = ["Clients"];
        public const int Version = 0;

        public IServiceCollection RegisterFeature(IServiceCollection services)
        {
            services.TryAddScoped<IUpdateClientService, UpdateClientService>();
            services.TryAddScoped<ICreateClientService, CreateClientService>();
            services.TryAddScoped<IGetClientService, GetClientService>();

            return services;
        }
    }
}
