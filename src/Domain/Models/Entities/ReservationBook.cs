using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    [Table("ReservationBook")]
    public class ReservationBook
    {
        public Guid ReservationId { get; set; }
        public Guid BookId { get; set; }

        [ForeignKey(nameof(ReservationId))]  public Reservation Reservation { get; set; }

        [ForeignKey(nameof(BookId))] public Book Book { get; set; }
    }
}
