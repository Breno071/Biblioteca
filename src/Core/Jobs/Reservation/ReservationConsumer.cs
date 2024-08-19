using Microsoft.Extensions.Hosting;

namespace Core.Jobs.Reservation
{
    public class ReservationConsumer : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
