using API.Features.Reservation.Services;
using API.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API.Features.Reservation
{
    public class ReservationFeature : IFeature
    {
        public static readonly string[] Tags = ["Reservations"];
        public const int Version = 0;

        public IServiceCollection RegisterFeature(IServiceCollection services)
        {
            services.TryAddScoped<IReservationService, ReservationService>();
            return services;
        }
    }
}
