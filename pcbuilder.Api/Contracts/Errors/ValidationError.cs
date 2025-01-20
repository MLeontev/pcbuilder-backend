namespace pcbuilder.Api.Contracts.Errors;

public class ValidationError
{
    public string Message { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
}