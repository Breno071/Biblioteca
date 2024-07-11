using FastEndpoints;
using FluentValidation;
using System.ComponentModel;

namespace API.Features.Book.Endpoints.GetBooksPaginated
{
    public class GetBooksPaginatedRequest
    {
        [DefaultValue(1)]
        public int Page { get; set; } = 1;

        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
    }

    public class GetBooksPaginatedRequestValidator : Validator<GetBooksPaginatedRequest>
    {
        public GetBooksPaginatedRequestValidator()
        {
            RuleFor(x => x.PageSize).LessThanOrEqualTo(1000).WithMessage("O tamanho da página precisa ser igual ou menor a 1000");
        }
    }
}
