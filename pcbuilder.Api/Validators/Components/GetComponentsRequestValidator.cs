using FluentValidation;
using pcbuilder.Api.Contracts.Components;

namespace pcbuilder.Api.Validators.Components;

public class GetComponentsRequestValidator : AbstractValidator<GetComponentsRequest>
{
    public GetComponentsRequestValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Размер страницы должен быть больше 0");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер страницы должен быть больше 0");
    }
}