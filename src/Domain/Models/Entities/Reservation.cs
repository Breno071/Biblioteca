using Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace Domain.Models.Entities
{
    [Table("Reservation")]
    public class Reservation
    {
        public event EventHandler<ReservationCreatedEventArgs> ReservationCreated;
        public event EventHandler<ReservationFinishedEventArgs> ReservationFinished;

        [Key]
        public Guid ReservationId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(Client.ClientId))]
        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public ICollection<Book> Books { get; set; }

        public DateTime ReservationDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;

        public virtual void OnReservationCreated()
            => ReservationCreated?.Invoke(this, new ReservationCreatedEventArgs(this));
        public virtual void OnFinishedReservation()
            => ReservationFinished?.Invoke(this, new ReservationFinishedEventArgs(this));
    }
}
