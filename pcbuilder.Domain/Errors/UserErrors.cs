using pcbuilder.Shared;

namespace pcbuilder.Domain.Errors;

public static class UserErrors
{
    public static readonly Error UsernameTaken = Error.Conflict(
        "Users.UsernameIsTaken", "Имя уже занято");

    public static readonly Error InvalidCredentials = Error.Unauthorized(
        "Users.InvalidCredentials", "Неверный логин или пароль");

    public static readonly Error InvalidToken = Error.Unauthorized(
        "Users.InvalidToken", "Неверный токен");
}