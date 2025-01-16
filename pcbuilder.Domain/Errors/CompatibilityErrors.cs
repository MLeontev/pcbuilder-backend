using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Errors;

public static class CompatibilityErrors
{
    public static CompatibilityError SocketMismatch(PcComponent component1, PcComponent component2) =>
        CompatibilityError.Problem(
            "Compatibility.SocketMismatch", 
            $"Не совпадает сокет между {component1.Name} и {component2.Name}");
}