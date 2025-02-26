using FluentValidation;
using pcbuilder.Api.Contracts.Builds;

namespace pcbuilder.Api.Validators.Builds;

public class GenerateBuildReportRequestValidator : AbstractValidator<GenerateBuildReportRequest>
{
    public GenerateBuildReportRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название обязательно")
            .MaximumLength(100).WithMessage("Название не должно быть длиннее 100 символов");
    }
}