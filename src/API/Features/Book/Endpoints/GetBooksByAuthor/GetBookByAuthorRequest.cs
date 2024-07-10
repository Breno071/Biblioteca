using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.GetBooksByAuthor
{
    public class GetBookByAuthorRequest
    {
        [Required]
        [FromRoute]
        public string Author { get; set; }
    }
}
