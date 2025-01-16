namespace pcbuilder.Domain.Errors;

public class CompatibilityError
{
    public CompatibilityError(string code, string message, CompatibilityErrorStatus status)
    {
        Code = code;
        Message = message;
        Status = status;
    }

    public string Code { get; set; }

    public string Message { get; set; }

    public CompatibilityErrorStatus Status { get; set; }

    public static CompatibilityError Problem(string code, string message) =>
        new(code, message, CompatibilityErrorStatus.Problem);
}

public enum CompatibilityErrorStatus
{
    Problem,
    Warning
}