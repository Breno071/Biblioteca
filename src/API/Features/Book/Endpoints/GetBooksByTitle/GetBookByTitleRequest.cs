using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.GetBooksByTitle
{
    public class GetBookByTitleRequest
    {
        [Required]
        [FromRoute]
        public string Title { get; set; }
    }
}
