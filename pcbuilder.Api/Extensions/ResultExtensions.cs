using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Errors;
using pcbuilder.Shared;

namespace pcbuilder.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToErrorResponse(this Result result)
    {
        if (result.IsSuccess) throw new InvalidOperationException();

        var response = new ErrorResponse
        {
            Code = result.Error.Code,
            Message = result.Error.Message
        };

        return new ObjectResult(response)
        {
            StatusCode = GetStatusCode(result.Error.Type)
        };
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}