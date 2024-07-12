using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.GetBooksByYear
{
    public class GetBookByYearRequest
    {
        [Required]
        [FromRoute]
        public int Year { get; set; }
    }
}
