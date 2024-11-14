using pcbuilder.Shared;

namespace pcbuilder.Domain.Errors;

public static class UserErrors
{
    public static Error UsernameTaken = Error.Conflict(
        "Users.UsernameIsTaken", $"Имя уже занято");
    
    public static Error InvalidCredentials = Error.Unauthorized(
        "Users.InvalidCredentials", $"Неверный логин или пароль");
    
    public static Error InvalidToken = Error.Unauthorized(
        "Users.InvalidToken", $"Неверный токен");
}