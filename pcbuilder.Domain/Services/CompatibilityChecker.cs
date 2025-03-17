using System.Globalization;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cases;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.PowerSupplies;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.Services;

public class CompatibilityChecker
{
    public CompatibilityResult CheckRequiredComponents(BuildWithComponents build)
    {
        var result = new CompatibilityResult();
        var missingComponents = new List<string>();

        if (build.Cpu == null) missingComponents.Add("Процессор");
        
        if (build.Motherboard == null) missingComponents.Add("Материнская плата");
        
        if (build.Cooler == null) missingComponents.Add("Охлаждение процессора");
        
        if (build.Rams == null || !build.Rams.Any()) missingComponents.Add("Память");
        
        if (build.Case == null) missingComponents.Add("Корпус");
        
        if (build.Psu == null) missingComponents.Add("Блок питания");
        
        if (build.Gpu == null && build.Cpu?.IntegratedGpu == null) missingComponents.Add("Видеокарта");
        
        if (build.Storages == null || build.Storages.Count == 0) missingComponents.Add("Накопитель");
        
        if (missingComponents.Count != 0)
        {
            result.AddError(CompatibilityErrors.MissingRequiredComponentsError(missingComponents));
        }

        return result;
    }
    
    public CompatibilityResult CheckCpuAndMotherboardCompatibility(Cpu? cpu, Motherboard? motherboard)
    {
        var result = new CompatibilityResult();

        if (cpu == null || motherboard == null) return result;

        if (cpu.SocketId == motherboard.SocketId) return result;

        result.AddError(CompatibilityErrors.CpuMotherboardSocketMismatch(cpu, motherboard));

        return result;
    }

    public CompatibilityResult CheckCpuAndRamCompatibility(Cpu? cpu, List<Ram>? rams)
    {
        var result = new CompatibilityResult();

        if (cpu == null || rams == null || rams.Count == 0) return result;

        foreach (var ram in rams)
        {
            var supportedMemory = cpu.CpuMemories
                .FirstOrDefault(cm => cm.MemoryTypeId == ram.MemoryTypeId);

            if (supportedMemory == null)
                result.AddError(CompatibilityErrors.CpuRamTypeMismatch(cpu, ram));
            else if (ram.Frequency > supportedMemory.MaxMemorySpeed)
                result.AddError(CompatibilityErrors.CpuRamSpeedExceeded(cpu, ram));
        }

        var totalRamCapacity = rams.Sum(ram => ram.TotalCapacity);
        if (totalRamCapacity > cpu.MaxMemoryCapacity)
            result.AddError(CompatibilityErrors.CpuRamCapacityExceeded(cpu, totalRamCapacity));

        return result;
    }

    public CompatibilityResult CheckMotherboardAndRamCompatibility(Motherboard? motherboard, List<Ram>? rams)
    {
        var result = new CompatibilityResult();

        if (motherboard == null || rams == null || rams.Count == 0) return result;

        var compatibleRams = new List<Ram>();

        foreach (var ram in rams)
            if (ram.MemoryTypeId != motherboard.MemoryTypeId)
                result.AddError(CompatibilityErrors.MotherboardRamTypeMismatch(motherboard, ram));
            else
                compatibleRams.Add(ram);

        var totalRamModules = compatibleRams.Sum(r => r.Modules);
        if (totalRamModules > motherboard.MemorySlots)
            result.AddError(CompatibilityErrors.MotherboardRamSlotLimitExceeded(motherboard, totalRamModules));

        var totalRamCapacity = compatibleRams.Sum(ram => ram.TotalCapacity);
        if (totalRamCapacity > motherboard.MaxMemoryCapacity)
            result.AddError(CompatibilityErrors.MotherboardRamCapacityExceeded(motherboard, totalRamCapacity));

        foreach (var ram in compatibleRams)
            if (ram.Frequency > motherboard.MaxMemorySpeed)
                result.AddError(CompatibilityErrors.MotherboardRamSpeedExceeded(motherboard, ram));

        return result;
    }

    public CompatibilityResult CheckCpuAndCoolerCompatibility(Cpu? cpu, Cooler? cooler)
    {
        var result = new CompatibilityResult();

        if (cpu == null || cooler == null) return result;

        if (cooler.CoolerSockets.All(cs => cs.SocketId != cpu.SocketId))
            result.AddError(CompatibilityErrors.CpuCoolerSocketMismatch(cpu, cooler));

        if (cooler.Tdp < cpu.Tdp) result.AddError(CompatibilityErrors.CpuCoolerTdpMismatch(cpu, cooler));

        return result;
    }

    public CompatibilityResult CheckMotherboardAndStorageCompatibility(Motherboard? motherboard,
        List<Storage>? storages)
    {
        var result = new CompatibilityResult();

        if (motherboard == null || storages == null || storages.Count == 0) return result;

        var sataStorages = storages.Where(s => s.StorageInterface.Name == "SATA").ToList();
        var availableSataSlots = motherboard.MotherboardStorages
            .Where(ms => ms.SupportedInterfaces.Any(si => si.StorageInterface.Name == "SATA"))
            .Sum(ms => ms.Quantity);
        if (sataStorages.Count > availableSataSlots)
            result.AddError(CompatibilityErrors.NotEnoughSataPorts(availableSataSlots, sataStorages.Count));

        var m2Storages = storages.Where(s => s.StorageInterface.Name.StartsWith("M.2")).ToList();
        var availableM2Slots = new List<(List<int> InterfaceIds, List<int> FormFactorIds, bool IsTaken)>();
        foreach (var motherboardStorage in motherboard.MotherboardStorages)
            if (motherboardStorage.SupportedInterfaces.Any(ms => ms.StorageInterface.Name.StartsWith("M.2")))
                for (var i = 0; i < motherboardStorage.Quantity; i++)
                {
                    var interfaceIds = motherboardStorage.SupportedInterfaces
                        .Select(si => si.StorageInterfaceId)
                        .ToList();
                    var formFactorIds = motherboardStorage.SupportedFormFactors
                        .Select(sf => sf.StorageFormFactorId)
                        .ToList();
                    availableM2Slots.Add((interfaceIds, formFactorIds, false));
                }

        var storageCompatibleSlots = new Dictionary<int, List<int>>();
        for (var i = 0; i < m2Storages.Count; i++)
        {
            var storage = m2Storages[i];
            var compatibleSlots = new List<int>();
            for (var j = 0; j < availableM2Slots.Count; j++)
            {
                var slot = availableM2Slots[j];
                if (slot.InterfaceIds.Contains(storage.StorageInterfaceId) &&
                    slot.FormFactorIds.Contains(storage.StorageFormFactorId))
                    compatibleSlots.Add(j);
            }

            storageCompatibleSlots[i] = compatibleSlots;
        }

        var storageIndicesSorted = storageCompatibleSlots
            .OrderBy(kvp => kvp.Value.Count)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var storageIdx in storageIndicesSorted)
        {
            var storage = m2Storages[storageIdx];
            var compatibleSlots = storageCompatibleSlots[storageIdx];
            var assigned = false;

            foreach (var slotIdx in compatibleSlots)
                if (!availableM2Slots[slotIdx].IsTaken)
                {
                    var slot = availableM2Slots[slotIdx];
                    availableM2Slots[slotIdx] = (slot.InterfaceIds, slot.FormFactorIds, true);
                    assigned = true;
                    break;
                }

            if (!assigned) result.AddError(CompatibilityErrors.NoCompatibleM2Slot(storage));
        }

        return result;
    }

    public CompatibilityResult CheckMotherboardAndGpuCompatibility(Motherboard? motherboard, Gpu? gpu)
    {
        var result = new CompatibilityResult();

        if (motherboard == null || gpu == null) return result;

        if (!string.IsNullOrEmpty(motherboard.PcieVersion) && !string.IsNullOrEmpty(gpu.PcieVersion))
        {
            var motherboardPcieVersion = float.Parse(motherboard.PcieVersion, CultureInfo.InvariantCulture);
            var gpuPcieVersion = float.Parse(gpu.PcieVersion, CultureInfo.InvariantCulture);

            if (gpuPcieVersion > motherboardPcieVersion)
                result.AddError(CompatibilityErrors.GpuMotherboardPcieVersionMismatch(motherboard, gpu));
        }

        if (motherboard.PcieSlotsCount <= 0) result.AddError(CompatibilityErrors.NoAvailablePcieSlots(motherboard));

        return result;
    }

    public CompatibilityResult CheckCaseAndMotherboardCompatibility(Case? pcCase, Motherboard? motherboard)
    {
        var result = new CompatibilityResult();

        if (pcCase == null || motherboard == null) return result;

        if (motherboard.FormFactor.Rank > pcCase.MaxMotherboardFormFactor.Rank)
            result.AddError(CompatibilityErrors.CaseMotherboardFormFactorMismatch(pcCase, motherboard));

        return result;
    }

    public CompatibilityResult CheckCaseAndGpuCompatibility(Case? pcCase, Gpu? gpu)
    {
        var result = new CompatibilityResult();

        if (pcCase == null || gpu == null) return result;

        if (gpu.Length > pcCase.MaxGpuLength) result.AddError(CompatibilityErrors.GpuTooLongForCase(gpu, pcCase));

        return result;
    }

    public CompatibilityResult CheckCaseAndStorageCompatibility(Case? pcCase, List<Storage>? storages)
    {
        var result = new CompatibilityResult();

        if (pcCase == null || storages == null || storages.Count == 0) return result;

        var requiredSlots25 = storages.Count(s => s.StorageFormFactor.Id == 1);
        var requiredSlots35 = storages.Count(s => s.StorageFormFactor.Id == 2);

        if (requiredSlots25 > pcCase.Slots25)
            result.AddError(CompatibilityErrors.NotEnoughStorageSlotsForCase(
                "2.5\"", requiredSlots25, pcCase.Slots25, pcCase));

        if (requiredSlots35 > pcCase.Slots35)
            result.AddError(CompatibilityErrors.NotEnoughStorageSlotsForCase(
                "3.5\"", requiredSlots35, pcCase.Slots35, pcCase));

        return result;
    }

    public CompatibilityResult CheckCaseAndAirCoolerCompatibility(Case? pcCase, Cooler? cooler)
    {
        var result = new CompatibilityResult();

        if (cooler == null || pcCase == null) return result;

        if (cooler.Height.HasValue && cooler.Height.Value > pcCase.MaxCoolerHeight)
            result.AddError(CompatibilityErrors.CoolerTooTallForCase(pcCase, cooler));

        return result;
    }

    public CompatibilityResult CheckCaseAndWaterCoolerCompatibility(Case? pcCase, Cooler? cooler)
    {
        var result = new CompatibilityResult();

        if (cooler == null || pcCase == null) return result;

        if (cooler.WaterCoolingSizeId.HasValue)
        {
            var caseWaterCoolingSize = pcCase.CaseWaterCoolingSizes
                .FirstOrDefault(x => x.WaterCoolingSizeId == cooler.WaterCoolingSizeId.Value);

            if (caseWaterCoolingSize == null)
                result.AddError(CompatibilityErrors.WaterCoolingSizeNotSupported(pcCase, cooler));
        }

        return result;
    }

    public CompatibilityResult CheckCaseAndPowerSupplyCompatibility(Case? pcCase, PowerSupply? psu)
    {
        var result = new CompatibilityResult();

        if (pcCase == null || psu == null) return result;

        if (psu.Length > pcCase.MaxPsuLength) result.AddError(CompatibilityErrors.PsuTooLongForCase(pcCase, psu));

        return result;
    }

    public CompatibilityResult CheckPsuAndStoragesCompatibility(PowerSupply? psu, List<Storage>? storages)
    {
        var result = new CompatibilityResult();

        if (psu == null || storages == null || storages.Count == 0) return result;

        var requiredSataConnectors = storages.Count(s => s.StorageInterfaceId == 1);
        var availableSataConnectors = psu.PsuPowerConnectors
            .FirstOrDefault(pc => pc.PowerConnector.Id == 3)?.Quantity ?? 0;

        if (requiredSataConnectors > availableSataConnectors)
            result.AddError(
                CompatibilityErrors.NotEnoughSataConnectors(psu, requiredSataConnectors, availableSataConnectors));

        return result;
    }

    public CompatibilityResult CheckPsuAndMotherboardCompatibility(PowerSupply? psu, Motherboard? motherboard)
    {
        var result = new CompatibilityResult();

        if (psu == null || motherboard == null) return result;

        foreach (var requiredConnector in motherboard.MotherboardPowerConnectors)
        {
            var requiredId = requiredConnector.PowerConnector.Id;
            var requiredQuantity = requiredConnector.Quantity;
            
            var availableQuantity = 0;
            
            var psuConnector = psu.PsuPowerConnectors
                .FirstOrDefault(pc => pc.PowerConnector.Id == requiredId);
            
            if (psuConnector != null)
                availableQuantity += psuConnector.Quantity;
            
            var compatibleConnectors = psu.PsuPowerConnectors
                .Where(pc => pc.PowerConnector.CompatibleConnectors
                    .Any(pcc => pcc.CompatibleConnectorId == requiredId)).ToList();

            foreach (var compatiblePsuConnector in compatibleConnectors)
            {
                var requiredPerUnit = compatiblePsuConnector.PowerConnector.CompatibleConnectors
                    .First(c => c.CompatibleConnectorId == requiredId).RequiredQuantity;

                availableQuantity += compatiblePsuConnector.Quantity / requiredPerUnit;
            }

            if (availableQuantity < requiredQuantity)
            {
                result.AddError(CompatibilityErrors.MissingPowerConnectorMotherboard(
                    psu, 
                    motherboard, 
                    requiredConnector.PowerConnector.Name, 
                    requiredQuantity, 
                    availableQuantity));
            }
        }
        
        return result;
    }
    
    public CompatibilityResult CheckPsuAndGpuCompatibility(PowerSupply? psu, Gpu? gpu)
    {
        var result = new CompatibilityResult();

        if (psu == null || gpu == null) return result;

        foreach (var requiredConnector in gpu.GpuPowerConnectors)
        {
            var requiredId = requiredConnector.PowerConnector.Id;
            var requiredQuantity = requiredConnector.Quantity;
            
            var availableQuantity = 0;
            
            var psuConnector = psu.PsuPowerConnectors
                .FirstOrDefault(pc => pc.PowerConnector.Id == requiredId);
            
            if (psuConnector != null)
                availableQuantity += psuConnector.Quantity;
            
            var compatibleConnectors = psu.PsuPowerConnectors
                .Where(pc => pc.PowerConnector.CompatibleConnectors
                    .Any(pcc => pcc.CompatibleConnectorId == requiredId)).ToList();

            foreach (var compatiblePsuConnector in compatibleConnectors)
            {
                var requiredPerUnit = compatiblePsuConnector.PowerConnector.CompatibleConnectors
                    .First(c => c.CompatibleConnectorId == requiredId).RequiredQuantity;

                availableQuantity += compatiblePsuConnector.Quantity / requiredPerUnit;
            }

            if (availableQuantity < requiredQuantity)
            {
                result.AddError(CompatibilityErrors.MissingPowerConnectorGpu(
                    psu, 
                    gpu, 
                    requiredConnector.PowerConnector.Name, 
                    requiredQuantity, 
                    availableQuantity));
            }
        }
        
        return result;
    }

    public CompatibilityResult CheckPsuPower(PowerSupply? psu, Cpu? cpu, Gpu? gpu, List<Ram>? rams, List<Storage>? storages)
    {
        var result = new CompatibilityResult();

        if (psu == null) return result;

        var cpuPower = cpu?.Tdp ?? 0;
        var gpuPower = gpu?.Tdp ?? 0;
        var ramPower = rams?.Sum(r => r.Modules * 5) ?? 0;
        var sataPower = (storages?.Count(s => s.StorageInterfaceId == 1) ?? 0) * 15;
        var m2Power = (storages?.Count(s => s.StorageInterfaceId is 2 or 3) ?? 0) * 9;
        
        var totalPower = cpuPower + gpuPower + ramPower + sataPower + m2Power;
        var recommendedPower = (int)(totalPower * 1.3);

        if (recommendedPower > psu.Power)
        {
            result.AddError(CompatibilityErrors.PsuPowerLowForSystem(
                psu, 
                recommendedPower));
        }

        if (gpu != null && gpu.RecommendedPsuPower > psu.Power)
        {
            result.AddError(CompatibilityErrors.PsuPowerLowForGpu(
                psu, 
                gpu, 
                gpu.RecommendedPsuPower));
        }
        
        return result;
    }

    public CompatibilityResult CheckBuildCompatibility(BuildWithComponents build)
    {
        var result = new CompatibilityResult();

        result.AddErrors(CheckRequiredComponents(build).Errors);
        
        result.AddErrors(CheckCpuAndMotherboardCompatibility(build.Cpu, build.Motherboard).Errors);
        result.AddErrors(CheckCpuAndRamCompatibility(build.Cpu, build.Rams).Errors);
        result.AddErrors(CheckMotherboardAndRamCompatibility(build.Motherboard, build.Rams).Errors);
        result.AddErrors(CheckCpuAndCoolerCompatibility(build.Cpu, build.Cooler).Errors);
        result.AddErrors(CheckMotherboardAndStorageCompatibility(build.Motherboard, build.Storages).Errors);
        result.AddErrors(CheckMotherboardAndGpuCompatibility(build.Motherboard, build.Gpu).Errors);
        
        result.AddErrors(CheckCaseAndMotherboardCompatibility(build.Case, build.Motherboard).Errors);
        result.AddErrors(CheckCaseAndGpuCompatibility(build.Case, build.Gpu).Errors);
        result.AddErrors(CheckCaseAndStorageCompatibility(build.Case, build.Storages).Errors);
        result.AddErrors(CheckCaseAndAirCoolerCompatibility(build.Case, build.Cooler).Errors);
        result.AddErrors(CheckCaseAndWaterCoolerCompatibility(build.Case, build.Cooler).Errors);
        result.AddErrors(CheckCaseAndPowerSupplyCompatibility(build.Case, build.Psu).Errors);

        result.AddErrors(CheckPsuAndStoragesCompatibility(build.Psu, build.Storages).Errors);
        result.AddErrors(CheckPsuAndMotherboardCompatibility(build.Psu, build.Motherboard).Errors);
        result.AddErrors(CheckPsuAndGpuCompatibility(build.Psu, build.Gpu).Errors);
        result.AddErrors(CheckPsuPower(build.Psu, build.Cpu, build.Gpu, build.Rams, build.Storages).Errors);

        return result;
    }

    public bool IsRamCompatible(BuildWithComponents buildWithComponents)
    {
        var cpuRamResult = CheckCpuAndRamCompatibility(buildWithComponents.Cpu, buildWithComponents.Rams);
        var motherboardRamResult = CheckMotherboardAndRamCompatibility(buildWithComponents.Motherboard, buildWithComponents.Rams);
    
        return cpuRamResult.Status != CompatibilityStatus.Incompatible && 
               motherboardRamResult.Status != CompatibilityStatus.Incompatible;
    }

    public bool IsCpuCompatible(BuildWithComponents buildWithComponents)
    {
        var cpuMotherboardResult = CheckCpuAndMotherboardCompatibility(buildWithComponents.Cpu, buildWithComponents.Motherboard);
        var cpuCoolerResult = CheckCpuAndCoolerCompatibility(buildWithComponents.Cpu, buildWithComponents.Cooler);
        var cpuRamResult = CheckCpuAndRamCompatibility(buildWithComponents.Cpu, buildWithComponents.Rams);
    
        return cpuMotherboardResult.Status != CompatibilityStatus.Incompatible && 
               cpuCoolerResult.Status != CompatibilityStatus.Incompatible && 
               cpuRamResult.Status != CompatibilityStatus.Incompatible;
    }
    
    public bool IsMotherboardCompatible(BuildWithComponents buildWithComponents)
    {
        var cpuMotherboardResult = CheckCpuAndMotherboardCompatibility(buildWithComponents.Cpu, buildWithComponents.Motherboard);
        var ramMotherboardResult = CheckMotherboardAndRamCompatibility(buildWithComponents.Motherboard, buildWithComponents.Rams);
        var gpuMotherboardResult = CheckMotherboardAndGpuCompatibility(buildWithComponents.Motherboard, buildWithComponents.Gpu);
        var storageMotherboardResult = CheckMotherboardAndStorageCompatibility(buildWithComponents.Motherboard, buildWithComponents.Storages);
        var psuMotherboardResult = CheckPsuAndMotherboardCompatibility(buildWithComponents.Psu, buildWithComponents.Motherboard);
        var caseMotherboardResult = CheckCaseAndMotherboardCompatibility(buildWithComponents.Case, buildWithComponents.Motherboard);
    
        return cpuMotherboardResult.Status != CompatibilityStatus.Incompatible && 
               ramMotherboardResult.Status != CompatibilityStatus.Incompatible && 
               gpuMotherboardResult.Status != CompatibilityStatus.Incompatible && 
               storageMotherboardResult.Status != CompatibilityStatus.Incompatible &&
               psuMotherboardResult.Status != CompatibilityStatus.Incompatible &&
               caseMotherboardResult.Status != CompatibilityStatus.Incompatible;
    }
    
    public bool IsGpuCompatible(BuildWithComponents buildWithComponents)
    {
        var motherboardGpuResult = CheckMotherboardAndGpuCompatibility(buildWithComponents.Motherboard, buildWithComponents.Gpu);
        var caseGpuResult = CheckCaseAndGpuCompatibility(buildWithComponents.Case, buildWithComponents.Gpu);
        var psuGpuResult = CheckPsuAndGpuCompatibility(buildWithComponents.Psu, buildWithComponents.Gpu);

        return motherboardGpuResult.Status != CompatibilityStatus.Incompatible && 
               caseGpuResult.Status != CompatibilityStatus.Incompatible && 
               psuGpuResult.Status != CompatibilityStatus.Incompatible;
    }
    
    public bool IsCoolerCompatible(BuildWithComponents buildWithComponents)
    {
        var cpuCoolerResult = CheckCpuAndCoolerCompatibility(buildWithComponents.Cpu, buildWithComponents.Cooler);
        var caseCoolerResult = CheckCaseAndAirCoolerCompatibility(buildWithComponents.Case, buildWithComponents.Cooler);
        var caseWaterCoolerResult = CheckCaseAndWaterCoolerCompatibility(buildWithComponents.Case, buildWithComponents.Cooler);

        return cpuCoolerResult.Status != CompatibilityStatus.Incompatible && 
               caseCoolerResult.Status != CompatibilityStatus.Incompatible && 
               caseWaterCoolerResult.Status != CompatibilityStatus.Incompatible;
    }
    
    public bool IsPsuCompatible(BuildWithComponents buildWithComponents)
    {
        var psuMotherboardResult = CheckPsuAndMotherboardCompatibility(buildWithComponents.Psu, buildWithComponents.Motherboard);
        var psuGpuResult = CheckPsuAndGpuCompatibility(buildWithComponents.Psu, buildWithComponents.Gpu);
        var psuStorageResult = CheckPsuAndStoragesCompatibility(buildWithComponents.Psu, buildWithComponents.Storages);
        var psuPowerResult = CheckPsuPower(buildWithComponents.Psu, buildWithComponents.Cpu, buildWithComponents.Gpu, buildWithComponents.Rams, buildWithComponents.Storages);
        var psuCaseResult = CheckCaseAndPowerSupplyCompatibility(buildWithComponents.Case, buildWithComponents.Psu);

        return psuMotherboardResult.Status != CompatibilityStatus.Incompatible && 
               psuGpuResult.Status != CompatibilityStatus.Incompatible && 
               psuStorageResult.Status != CompatibilityStatus.Incompatible && 
               psuPowerResult.Status != CompatibilityStatus.Incompatible && 
               psuCaseResult.Status != CompatibilityStatus.Incompatible;
    }
    
    public bool IsCaseCompatible(BuildWithComponents buildWithComponents)
    {
        var caseMotherboardResult = CheckCaseAndMotherboardCompatibility(buildWithComponents.Case, buildWithComponents.Motherboard);
        var caseGpuResult = CheckCaseAndGpuCompatibility(buildWithComponents.Case, buildWithComponents.Gpu);
        var caseStorageResult = CheckCaseAndStorageCompatibility(buildWithComponents.Case, buildWithComponents.Storages);
        var caseAirCoolerResult = CheckCaseAndAirCoolerCompatibility(buildWithComponents.Case, buildWithComponents.Cooler);
        var caseWaterCoolerResult = CheckCaseAndWaterCoolerCompatibility(buildWithComponents.Case, buildWithComponents.Cooler);
        var casePsuResult = CheckCaseAndPowerSupplyCompatibility(buildWithComponents.Case, buildWithComponents.Psu);

        return caseMotherboardResult.Status != CompatibilityStatus.Incompatible && 
               caseGpuResult.Status != CompatibilityStatus.Incompatible && 
               caseStorageResult.Status != CompatibilityStatus.Incompatible && 
               caseAirCoolerResult.Status != CompatibilityStatus.Incompatible && 
               caseWaterCoolerResult.Status != CompatibilityStatus.Incompatible && 
               casePsuResult.Status != CompatibilityStatus.Incompatible;
    }
    
    public bool IsStorageCompatible(BuildWithComponents buildWithComponents)
    {
        var motherboardStorageResult = CheckMotherboardAndStorageCompatibility(buildWithComponents.Motherboard, buildWithComponents.Storages);
        var psuStorageResult = CheckPsuAndStoragesCompatibility(buildWithComponents.Psu, buildWithComponents.Storages);
        var caseStorageResult = CheckCaseAndStorageCompatibility(buildWithComponents.Case, buildWithComponents.Storages);

        return motherboardStorageResult.Status != CompatibilityStatus.Incompatible && 
               psuStorageResult.Status != CompatibilityStatus.Incompatible &&
               caseStorageResult.Status != CompatibilityStatus.Incompatible;
    }
}