using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace Domain.Models.Entities
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public Guid BookId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O campo Title deve ser preenchido.")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required(ErrorMessage = "O campo Author deve ser preenchido.")]
        [MaxLength(255)]
        public string Author { get; set; }

        [Required(ErrorMessage = "O campo Year deve ser preenchido.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "O campo Publisher deve ser preenchido.")]
        [MaxLength(255)]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "O campo Genre deve ser preenchido.")]
        public Genre Genre { get; set; }

        public int Stock { get; set; } = 0;
        public bool Active { get; set; }  = true;

        public ICollection<Reservation> Reservations { get; set; }
    }
}
