using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Client.Endpoints.GetClientById
{
    public class GetClientByIdRequest
    {
        [Required]
        [FromRoute]
        public Guid ClientId { get; set; }
    }
}
