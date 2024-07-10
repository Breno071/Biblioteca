using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.DeleteBook
{
    public class DeleteBookRequest
    {
        [Required]
        [FromRoute]
        public Guid BookId { get; set; }
    }
}
