using FluentValidation;
using pcbuilder.Api.Contracts.Builds;

namespace pcbuilder.Api.Validators.Builds;

public class SaveBuildRequestValidator : AbstractValidator<SaveBuildRequest>
{
    public SaveBuildRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название обязательно")
            .MaximumLength(100).WithMessage("Название не должно быть длинее 100 символов");
    }
}