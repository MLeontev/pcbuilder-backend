using pcbuilder.Shared;

namespace pcbuilder.Domain.Errors;

public class BuildErrors
{
    public static Error DuplicateName => Error.Conflict(
        "Builds.DuplicateName", "Сборка с таким названием уже сохранена. Измените название сборки");
    
    public static Error ForbiddenAccess => Error.Forbidden(
        "Builds.ForbiddenAccess", "У вас нет прав на доступ к этой сборке");

    public static Error NotFound(int id) =>
        Error.NotFound("Builds.NotFound", $"Сборка с id={id} не найдена");
}