using Domain.Enums;
using FastEndpoints;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.CreateBook
{
    public class CreateBookRequest
    {
        [Required]
        [MaxLength(255)]
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
