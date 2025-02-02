using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;

namespace pcbuilder.Domain.Services;

public static class CompatibilityErrors
{
    public static CompatibilityError CpuMotherboardSocketMismatch(Cpu cpu, Motherboard motherboard)
    {
        return CompatibilityError.Problem(
            "Cpu.Motherboard.SocketMismatch",
            $"Сокет процессора ({cpu.Socket.Name}) и материнской платы ({motherboard.Socket.Name}) не совпадает");
    }

    public static CompatibilityError CpuRamTypeMismatch(Cpu cpu, Ram ram)
    {
        return CompatibilityError.Problem(
            "Cpu.Ram.RamTypeMismatch",
            $"Тип ОЗУ {ram.MemoryType.Name} ({ram.FullName}) не поддерживается процессором");
    }

    public static CompatibilityError CpuRamSpeedExceeded(Cpu cpu, Ram ram)
    {
        return CompatibilityError.Warning(
            "Cpu.Ram.SpeedExceeded",
            $"Частота ОЗУ {ram.Frequency} МГц ({ram.FullName}) превышает максимальную поддерживаемую процессором. " +
            "Память может работать на пониженной частоте");
    }

    public static CompatibilityError CpuRamCapacityExceeded(Cpu cpu, int totalRamCapacity)
    {
        return CompatibilityError.Problem(
            "Cpu.Ram.CapacityExceeded",
            $"Общий объем ОЗУ ({totalRamCapacity} ГБ) превышает лимит процессора ({cpu.MaxMemoryCapacity} ГБ).");
    }

    public static CompatibilityError MotherboardRamTypeMismatch(Motherboard motherboard, Ram ram)
    {
        return CompatibilityError.Problem(
            "Motherboard.Ram.RamTypeMismatch",
            $"Тип ОЗУ {ram.MemoryType.Name} ({ram.FullName}) не поддерживается материнской платой");
    }

    public static CompatibilityError MotherboardRamSlotLimitExceeded(Motherboard motherboard, int totalRamModules)
    {
        return CompatibilityError.Problem("Motherboard.Ram.SlotLimitExceeded",
            $"Количество модулей ОЗУ ({totalRamModules}) превышает доступные слоты ({motherboard.MemorySlots}).");
    }

    public static CompatibilityError MotherboardRamCapacityExceeded(Motherboard motherboard, int totalRamCapacity)
    {
        return CompatibilityError.Problem(
            "Motherboard.Ram.CapacityExceeded",
            $"Общий объем ОЗУ ({totalRamCapacity} ГБ) превышает лимит материнской платы ({motherboard.MaxMemoryCapacity} ГБ).");
    }

    public static CompatibilityError MotherboardRamSpeedExceeded(Motherboard motherboard, Ram ram)
    {
        return CompatibilityError.Warning(
            "Motherboard.Ram.SpeedExceeded",
            $"Частота ОЗУ {ram.Frequency} МГц ({ram.FullName}) превышает максимальную поддерживаемую материнской платой. " +
            "Память может работать на пониженной частоте");
    }

    public static CompatibilityError CpuCoolerSocketMismatch(Cpu cpu, Cooler cooler)
    {
        return CompatibilityError.Problem(
            "Cpu.Cooler.SocketMismatch",
            $"Сокет процессора ({cpu.Socket.Name}) не поддерживается системой охлаждения");
    }

    public static CompatibilityError CpuCoolerTdpMismatch(Cpu cpu, Cooler cooler)
    {
        return CompatibilityError.Problem(
            "Cpu.Cooler.TdpMismatch",
            $"TDP кулера ({cooler.Tdp}W) меньше TDP процессора ({cpu.Tdp}W. Возможен перегрев процессора");
    }
}