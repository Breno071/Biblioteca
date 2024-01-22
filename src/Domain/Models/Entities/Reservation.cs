using Domain.Events;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Domain.Models.Entities
{
    public class Reservation
    {
        public event EventHandler<ReservationCreatedEventArgs> ReservationCreated;
        public event EventHandler<ReservationFinishedEventArgs> ReservationFinished;

        [Key]
        public Guid Code { get; set; } = Guid.NewGuid();
        [Required]
        public Client Client { get; set; }
        [Required]
        public ICollection<Book> Books { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;

        public virtual void OnReservationCreated()
            => ReservationCreated?.Invoke(this, new ReservationCreatedEventArgs(this));
        public virtual void OnFinishedReservation()
            => ReservationFinished?.Invoke(this, new ReservationFinishedEventArgs(this));
    }
}
