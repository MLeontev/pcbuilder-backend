using FluentValidation.Results;
using pcbuilder.Api.Contracts;
using pcbuilder.Api.Contracts.Errors;

namespace pcbuilder.Api.Extensions;

public static class ValidationResultExtensions
{
    public static ValidationErrorResponse ToValidationErrorResponse(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(e => new ValidationError
        {
            Message = e.ErrorMessage,
            Field = e.PropertyName
        }).ToList();

        return new ValidationErrorResponse
        {
            Errors = errors
        };
    }
}