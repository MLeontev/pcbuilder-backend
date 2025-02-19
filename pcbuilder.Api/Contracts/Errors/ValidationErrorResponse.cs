namespace pcbuilder.Api.Contracts.Errors;

public class ValidationErrorResponse
{
    public string Code { get; set; } = "ValidationError";
    public string Message { get; set; } = "В запросе содержатся ошибки валидации";
    public List<ValidationError> Errors { get; set; } = [];
}