using pcbuilder.Domain.Models.Cases;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.PowerSupplies;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Models.Storage;

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
            $"TDP кулера ({cooler.Tdp} Вт) меньше TDP процессора ({cpu.Tdp}) Вт. Возможен перегрев процессора");
    }

    public static CompatibilityError NotEnoughSataPorts(int availableSlots, int requiredSlots)
    {
        return CompatibilityError.Problem(
            "Motherboard.Storage.NotEnoughSataPorts",
            $"Недостаточно SATA портов на материнской плате. Требуется: {requiredSlots}, доступно: {availableSlots}");
    }

    public static CompatibilityError NoCompatibleM2Slot(Storage storage)
    {
        return CompatibilityError.Problem(
            "Motherboard.Storage.NoCompatibleM2Slot",
            $"Для накопителя {storage.FullName} не найден совместимый слот M.2."
        );
    }

    public static CompatibilityError GpuMotherboardPcieVersionMismatch(Motherboard motherboard, Gpu gpu)
    {
        return CompatibilityError.Warning(
            "Gpu.Motherboard.PcieVersionMismatch",
            $"Версия PCI-Express видеокарты ({gpu.PcieVersion}) выше версии материнской платы ({motherboard.PcieVersion}). Возможно снижение производительности");
    }

    public static CompatibilityError NoAvailablePcieSlots(Motherboard motherboard)
    {
        return CompatibilityError.Problem(
            "Motherboard.NoAvailablePcieSlots",
            "У материнской платы нет слотов для подключения видеокарты");
    }

    public static CompatibilityError CaseMotherboardFormFactorMismatch(Case pcCase, Motherboard motherboard)
    {
        return CompatibilityError.Problem(
            "Case.Motherboard.FormFactorMismatch",
            $"Форм-фактор материнской платы ({motherboard.FormFactor.Name}) превышает максимально поддерживаемый размер для корпуса ({pcCase.MaxMotherboardFormFactor.Name})");
    }

    public static CompatibilityError GpuTooLongForCase(Gpu gpu, Case pcCase)
    {
        return CompatibilityError.Problem(
            "Gpu.Case.LengthMismatch",
            $"Длина видеокарты {gpu.Length} мм превышает максимальную допустимую длину для корпуса {pcCase.MaxGpuLength} мм");
    }

    public static CompatibilityError NotEnoughStorageSlotsForCase(string storageType, int required, int available,
        Case pcCase)
    {
        return CompatibilityError.Problem(
            "Case.Storage.SlotsMismatch",
            $"В корпусе недостаточно отсеков для {storageType} накопителей. Требуется: {required}, доступно: {available}");
    }

    public static CompatibilityError CoolerTooTallForCase(Case pcCase, Cooler cooler)
    {
        return CompatibilityError.Problem(
            "Cooler.Case.CoolerTooTall",
            $"Высота кулера {cooler.Height}мм превышает максимальную допустимую высоту для корпуса: {pcCase.MaxCoolerHeight}мм.");
    }

    public static CompatibilityError WaterCoolingSizeNotSupported(Case pcCase, Cooler cooler)
    {
        return CompatibilityError.Problem(
            "Cooler.Case.WaterCoolingSizeNotSupported",
            $"Размер водяного охлаждения {cooler.WaterCoolingSize?.Size} мм не поддерживается корпусом");
    }

    public static CompatibilityError PsuTooLongForCase(Case pcCase, PowerSupply psu)
    {
        return CompatibilityError.Problem(
            "Psu.Case.LengthMismatch",
            $"Длина блока питания {psu.Length} мм превышает максимальную допустимую длину для корпуса: {pcCase.MaxPsuLength} мм.");
    }

    public static CompatibilityError NotEnoughSataConnectors(PowerSupply psu, int required, int available)
    {
        return CompatibilityError.Problem(
            "Psu.Storage.NotEnoughSataConnectors",
            $"Недостаточно SATA-разъемов на блоке питания. Требуется: {required}, доступно: {available}");
    }

    public static CompatibilityError MissingPowerConnectorMotherboard(PowerSupply psu, Motherboard motherboard,
        string connectorName, int connectorValue, int availableQuantity)
    {
        return CompatibilityError.Problem(
            "Psu.Motherboard.MissingPowerConnector",
            $"Недостаточно разъемов питания ({connectorName}) для материнской платы. Требуется: {connectorValue}, доступно: {availableQuantity}");
    }

    public static CompatibilityError MissingPowerConnectorGpu(PowerSupply psu, Gpu gpu, 
        string connectorName, int connectorValue, int availableQuantity)
    {
        return CompatibilityError.Problem(
            "Psu.Motherboard.MissingPowerConnector",
            $"Недостаточно разъемов доп. питания ({connectorName}) для видеокарты. Требуется: {connectorValue}, доступно: {availableQuantity}");
    }

    public static CompatibilityError PsuPowerLowForSystem(PowerSupply psu, int recommendedPower)
    {
        return CompatibilityError.Problem(
            "Psu.System.PowerTooLow",
            $"Мощность блока питания {psu.Power} Вт недостаточна для системы. Рекомендуемая мощность: {recommendedPower} Вт");
    }

    public static CompatibilityError PsuPowerLowForGpu(PowerSupply psu, Gpu gpu, int recommendedGpuPower)
    {
        return CompatibilityError.Warning(
            "Psu.Gpu.PowerTooLow", 
            $"Мощность блока питания {psu.Power} Вт недостаточна для видеокарты. Рекомендуемая мощность: {recommendedGpuPower} Вт");
    }
    
    public static CompatibilityError MissingRequiredComponentsError(List<string> missingComponents)
    {
        return CompatibilityError.Note(
            "Build.RequiredComponentsMissing", 
            $"Отсутствуют обязательные комплектующие: {string.Join(", ", missingComponents)}");
    }
}