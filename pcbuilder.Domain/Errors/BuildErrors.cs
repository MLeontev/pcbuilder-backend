using pcbuilder.Shared;

namespace pcbuilder.Domain.Errors;

public class BuildErrors
{
    public static Error ForbiddenAccess => Error.Unauthorized(
        "Builds.ForbiddenAccess", "У вас нет прав на доступ к этой сборке");

    public static Error NotFound(int id)
    {
        return Error.NotFound(
            "Builds.NotFound", $"Сборка с id={id} не найдена");
    }
}