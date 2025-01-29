using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;

namespace pcbuilder.Domain.Services;

public static class CompatibilityErrors
{
    public static CompatibilityError CpuMotherboardSocketMismatch(Cpu cpu, Motherboard motherboard) =>
        CompatibilityError.Problem(
            "Cpu.Motherboard.SocketMismatch",
            $"Сокет процессора ({cpu.Socket.Name}) и материнской платы ({motherboard.Socket.Name}) не совпадает");

    public static CompatibilityError CpuRamTypeMismatch(Cpu cpu, Ram ram) =>
        CompatibilityError.Problem(
            "Cpu.Ram.RamTypeMismatch",
            $"Тип ОЗУ {ram.MemoryType.Name} ({ram.FullName}) не поддерживатеся процессором");

    public static CompatibilityError CpuRamSpeedExceeded(Cpu cpu, Ram ram) =>
        CompatibilityError.Warning(
            "Cpu.Ram.SpeedExceeded",
            $"Частота ОЗУ {ram.Frequency} МГц ({ram.FullName}) превышает максимальную поддерживаемую процессором." +
            "Память может работать на пониженной частоте");

    public static CompatibilityError CpuRamCapacityExceeded(Cpu cpu, int totalRamCapacity) =>
        CompatibilityError.Problem(
            "Cpu.Ram.CapacityExceeded",
            $"Общий объем ОЗУ ({totalRamCapacity} ГБ) превышает лимит процессора ({cpu.MaxMemoryCapacity} ГБ).");

    public static CompatibilityError MotherboardRamTypeMismatch(Motherboard motherboard, Ram ram) =>
        CompatibilityError.Problem(
            "Motherboard.Ram.RamTypeMismatch",
            $"Тип ОЗУ {ram.MemoryType.Name} ({ram.FullName}) не поддерживатеся материнской платой");

    public static CompatibilityError MotherboardRamSlotLimitExceeded(Motherboard motherboard, int totalRamModules) =>
        CompatibilityError.Problem("Motherboard.Ram.SlotLimitExceeded",
            $"Количество модулей ОЗУ ({totalRamModules}) превышает доступные слоты ({motherboard.MemorySlots}).");
    
    public static CompatibilityError MotherboardRamCapacityExceeded(Motherboard motherboard, int totalRamCapacity) =>
        CompatibilityError.Problem(
            "Motherboard.Ram.CapacityExceeded",
            $"Общий объем ОЗУ ({totalRamCapacity} ГБ) превышает лимит материнской платы ({motherboard.MaxMemoryCapacity} ГБ).");
    
    public static CompatibilityError MotherboardRamSpeedExceeded(Motherboard motherboard, Ram ram) =>
        CompatibilityError.Warning(
            "Motherboard.Ram.SpeedExceeded",
            $"Частота ОЗУ {ram.Frequency} МГц ({ram.FullName}) превышает максимальную поддерживаемую материнской платой." +
            "Память может работать на пониженной частоте");
}