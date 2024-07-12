using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.GetBooksByGenre
{
    public class GetBookByGenreRequest
    {
        [Required]
        [FromRoute]
        public Genre Genre { get; set; }
    }
}
