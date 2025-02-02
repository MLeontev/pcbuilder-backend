using FluentValidation;
using pcbuilder.Api.Contracts.Builds;

namespace pcbuilder.Api.Validators.Builds;

public class GetBuildsRequestValidator : AbstractValidator<GetBuildsRequest>
{
    public GetBuildsRequestValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Размер страницы должен быть больше 0");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер страницы должен быть больше 0");
    }
}