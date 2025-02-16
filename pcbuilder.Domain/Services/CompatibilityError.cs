namespace pcbuilder.Domain.Services;

public class CompatibilityError
{
    public CompatibilityError(string code, string message, CompatibilityErrorStatus status)
    {
        Code = code;
        Message = message;
        Status = status;
    }

    public string Code { get; }

    public string Message { get; }

    public CompatibilityErrorStatus Status { get; }

    public static CompatibilityError Note(string code, string message) => 
        new(code, message, CompatibilityErrorStatus.Note);

    public static CompatibilityError Warning(string code, string message) => 
        new(code, message, CompatibilityErrorStatus.Warning);

    public static CompatibilityError Problem(string code, string message) => 
        new(code, message, CompatibilityErrorStatus.Problem);
}

public enum CompatibilityErrorStatus
{
    Note = 0,
    Warning = 1,
    Problem = 2
}