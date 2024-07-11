using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace Domain.Models.Entities
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public Guid ClientId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O campo Name deve ser preenchido.")]
        [MaxLength(255, ErrorMessage = "O campo Name deve ter no máximo 255 caracteres.")]
        [MinLength(3, ErrorMessage = "O campo Name deve ter no mínimo 3 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Email deve ser preenchido.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [MaxLength(255, ErrorMessage = "O campo Email deve ter no máximo 255 caracteres.")]
        public string Email { get; set; }
    }
}
