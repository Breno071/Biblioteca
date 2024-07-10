using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Client.Endpoints.GetClientByEmail
{
    public class GetClientByEmailRequest
    {
        [Required]
        [FromRoute]
        public string Email { get; set; }
    }
}
