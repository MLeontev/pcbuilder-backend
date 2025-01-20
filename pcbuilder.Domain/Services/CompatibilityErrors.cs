using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.Services;

public static class CompatibilityErrors
{
    public static CompatibilityError CpuMotherboardSocketMismatch(Cpu cpu, Motherboard motherboard) =>
        CompatibilityError.Problem(
            "Compatibility.SocketMismatch", 
            $"Сокет процессора {cpu.FullName} ({cpu.Socket.Name}) и материнской платы {motherboard.FullName} ({motherboard.Socket.Name}) не совпадает");
}