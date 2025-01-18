using FluentValidation;
using pcbuilder.Api.Contracts.Users;

namespace pcbuilder.Api.Validators.Users;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be between 3 and 50 characters")
            .MaximumLength(50).WithMessage("Username must be between 3 and 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
        
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords don't match")
            .When(x => !string.IsNullOrEmpty(x.Password));
    }    
}