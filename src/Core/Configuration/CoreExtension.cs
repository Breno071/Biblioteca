using Core.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration
{
    public static class CoreExtension
    {
        public static IServiceCollection AddApplicationCore(this IServiceCollection services)
        {
            services.AddScoped<ISendMessage, SendMessageService>();

            return services;
        }
    }
}
