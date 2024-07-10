using Domain.Enums;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Book.Endpoints.UpdateBook
{
    public class UpdateBookRequest
    {
        [Required]
        [FromRoute]
        public Guid Code { get; set; }

        [Required(ErrorMessage = "O campo Title deve ser preenchido.")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required(ErrorMessage = "O campo Author deve ser preenchido.")]
        [MaxLength(255)]
        public string Author { get; set; }

        [Required(ErrorMessage = "O campo Year deve ser preenchido.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "O campo Publisher deve ser preenchido.")]
        [MaxLength(255)]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "O campo Genre deve ser preenchido.")]
        public Genre Genre { get; set; }
        public int Stock { get; set; }
    }

    public class UpdateBookRequestValidator : Validator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Stock).GreaterThan(0).WithMessage("O stock deve ser maior do que zero!");
        }
    }
}
