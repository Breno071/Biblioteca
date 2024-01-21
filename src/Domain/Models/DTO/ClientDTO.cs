#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTO
{
    public class ClientDTO
    {
        public Guid Code { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O campo Name deve ser preenchido.")]
        [MaxLength(255)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Email deve ser preenchido.")]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
    }
}
