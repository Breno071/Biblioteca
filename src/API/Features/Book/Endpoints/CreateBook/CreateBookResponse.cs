using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.CreateBook
{
    public class CreateBookResponse
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }
        public Genre Genre { get; set; }
    }
}
