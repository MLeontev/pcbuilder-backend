using pcbuilder.Shared;

namespace pcbuilder.Domain.Errors;

public static class UserErrors
{
    public static Error UsernameTaken = Error.Conflict(
        "Users.UsernameIsTaken", $"Имя уже занято");
}