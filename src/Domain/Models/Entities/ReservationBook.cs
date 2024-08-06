using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    [Table("ReservationBook")]
    public class ReservationBook
    {
        public Guid ReservationId { get; set; }
        public Guid BookId { get; set; }
    }
}
