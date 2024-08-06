using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace Domain.Models.Entities
{
    [Table("Reservation")]
    public class Reservation
    {        
        [Key]
        public Guid ReservationId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(Client.ClientId))] public virtual Guid ClientId { get; set; }

        public Client Client { get; set; }

        public ICollection<Book> Books { get; set; }
        public List<ReservationBook> ReservationBooks { get; set; }

        public DateTime ReservationDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;
    }
}
