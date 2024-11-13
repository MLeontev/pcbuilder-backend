namespace pcbuilder.Shared;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new("NullValue", "Null value was provided", ErrorType.Failure);
    
    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public string Code { get; set; }
    
    public string Message { get; set; }
    
    public ErrorType Type { get; set; }
    
    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);
    
    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);
    
    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);
    
    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);
    
    public static Error Unauthorized(string code, string message) =>
        new(code, message, ErrorType.Unauthorized);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4
}