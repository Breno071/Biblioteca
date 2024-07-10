using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.GetBook
{
    public class GetBookByIdRequest
    {
        [FromRoute]
        [Required]
        public Guid Code { get; set; }
    }
}
