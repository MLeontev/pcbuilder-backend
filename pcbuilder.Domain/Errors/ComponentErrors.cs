using pcbuilder.Shared;

namespace pcbuilder.Domain.Errors;

public static class ComponentErrors
{
    public static Error NotFound(int id) => Error.NotFound(
        "Components.NotFound", $"Комплектующие с id={id} не найдены"); 
}