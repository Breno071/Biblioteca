using Domain.Enums;

namespace API.Features.Book.DTOs
{
    public class BookDetailsDto
    {
        public Guid BookId { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }
        public Genre Genre { get; set; }
        public int Stock { get; set; } = 0;
    }
}
