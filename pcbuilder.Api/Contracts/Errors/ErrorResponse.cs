using pcbuilder.Shared;

namespace pcbuilder.Api.Contracts.Errors;

public class ErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public static ErrorResponse FromError(Error error)
    {
        return new ErrorResponse
        {
            Code = error.Code,
            Message = error.Message,
        };
    }
}