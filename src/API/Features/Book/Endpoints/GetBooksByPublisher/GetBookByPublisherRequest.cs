using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.GetBooksByPublisher
{
    public class GetBookByPublisherRequest
    {
        [Required]
        [FromRoute]
        public string Publisher { get; set; }
    }
}
