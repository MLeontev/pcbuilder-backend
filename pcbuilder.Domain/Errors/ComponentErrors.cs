using pcbuilder.Shared;

namespace pcbuilder.Domain.Errors;

public static class ComponentErrors
{
    public static Error NotFound(int id)
    {
        return Error.NotFound(
            "Components.NotFound", $"Комплектующие с id={id} не найдены");
    }

    public static Error NotFound()
    {
        return Error.NotFound(
            "Components.NotFound", "Не все комплектующие были найдены");
    }
}