namespace pcbuilder.Api.Contracts;

public class ValidationErrorResponse
{
    public string Message { get; set; } = "В запросе содержатся ошибки валидации";
    public List<ValidationError> Errors { get; set; } = [];
}