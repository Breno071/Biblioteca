using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTO
{
    public class BookDTO
    {
        public Guid Code { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(1)]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string Author { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [MaxLength(255)]
        public string Publisher { get; set; }

        public Genre Genre { get; set; }
    }
}
