using System.Globalization;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cases;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.Services;

public class CompatibilityChecker
{
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
        {
            if (ram.MemoryTypeId != motherboard.MemoryTypeId)
            {
                result.AddError(CompatibilityErrors.MotherboardRamTypeMismatch(motherboard, ram));
            }
            else
            {
                compatibleRams.Add(ram);
            }
        }

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
        {
            result.AddError(CompatibilityErrors.CpuCoolerSocketMismatch(cpu, cooler));
        }

        if (cooler.Tdp < cpu.Tdp)
        {
            result.AddError(CompatibilityErrors.CpuCoolerTdpMismatch(cpu, cooler));
        }

        return result;
    }

    public CompatibilityResult CheckMotherboardAndStorageCompatibility(Motherboard? motherboard, List<Storage>? storages)
    {
        var result = new CompatibilityResult();
        
        if (motherboard == null || storages == null || storages.Count == 0)
        {
            return result;
        }
        
        var sataStorages = storages.Where(s => s.StorageInterface.Name == "SATA").ToList();
        var availableSataSlots = motherboard.MotherboardStorages
            .Where(ms => ms.SupportedInterfaces.Any(si => si.StorageInterface.Name == "SATA"))
            .Sum(ms => ms.Quantity);
        if (sataStorages.Count > availableSataSlots)
            result.AddError(CompatibilityErrors.NotEnoughSataPorts(availableSataSlots, sataStorages.Count));
        
        var m2Storages = storages.Where(s => s.StorageInterface.Name.StartsWith("M.2")).ToList();
        var availableM2Slots = new List<(List<int> InterfaceIds, List<int> FormFactorIds, bool IsTaken)>();
        foreach (var motherboardStorage in motherboard.MotherboardStorages)
        {
            if (motherboardStorage.SupportedInterfaces.Any(ms => ms.StorageInterface.Name.StartsWith("M.2")))
            {
                for (var i = 0; i < motherboardStorage.Quantity; i++)
                {
                    var interfaceIds = motherboardStorage.SupportedInterfaces.Select(si => si.StorageInterfaceId).ToList();
                    var formFactorIds = motherboardStorage.SupportedFormFactors.Select(sf => sf.StorageFormFactorId).ToList();
                    availableM2Slots.Add((interfaceIds, formFactorIds, false));
                }
            }
        }
        
        var storageCompatibleSlots = new Dictionary<int, List<int>>();
        for (int i = 0; i < m2Storages.Count; i++)
        {
            var storage = m2Storages[i];
            var compatibleSlots = new List<int>();
            for (int j = 0; j < availableM2Slots.Count; j++)
            {
                var slot = availableM2Slots[j];
                if (slot.InterfaceIds.Contains(storage.StorageInterfaceId) && 
                    slot.FormFactorIds.Contains(storage.StorageFormFactorId))
                {
                    compatibleSlots.Add(j);
                }
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
            bool assigned = false;
        
            foreach (var slotIdx in compatibleSlots)
            {
                if (!availableM2Slots[slotIdx].IsTaken)
                {
                    var slot = availableM2Slots[slotIdx];
                    availableM2Slots[slotIdx] = (slot.InterfaceIds, slot.FormFactorIds, true);
                    assigned = true;
                    break;
                }
            }
        
            if (!assigned)
            {
                result.AddError(CompatibilityErrors.NoCompatibleM2Slot(storage));
            }
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
            {
                result.AddError(CompatibilityErrors.GpuMotherboardPcieVersionMismatch(motherboard, gpu));
            }
        }

        if (motherboard.PcieSlotsCount <= 0)
        {
            result.AddError(CompatibilityErrors.NoAvailablePcieSlots(motherboard));
        }
        
        return result;
    }

    public CompatibilityResult CheckCaseAndMotherboardCompatibility(Case? pcCase, Motherboard? motherboard)
    {
        var result = new CompatibilityResult();
        
        if (pcCase == null || motherboard == null) return result;

        if (motherboard.FormFactor.Rank > pcCase.MaxMotherboardFormFactor.Rank)
        {
            result.AddError(CompatibilityErrors.CaseMotherboardFormFactorMismatch(pcCase, motherboard));
        }
        
        return result;
    }
    
    public CompatibilityResult CheckCaseAndGpuCompatibility(Case? pcCase, Gpu? gpu)
    {
        var result = new CompatibilityResult();
        
        if (pcCase == null || gpu == null) return result;

        if (gpu.Length > pcCase.MaxGpuLength)
        {
            result.AddError(CompatibilityErrors.GpuTooLongForCase(gpu, pcCase));
        }
        
        return result;
    }

    public CompatibilityResult CheckCaseAndStorageCompatibility(Case? pcCase, List<Storage>? storages)
    {
        var result = new CompatibilityResult();

        if (pcCase == null || storages == null || storages.Count == 0) return result;

        return result;
    }
    
    public CompatibilityResult CheckBuildCompatibility(BuildWithComponents build)
    {
        var result = new CompatibilityResult();

        result.AddErrors(CheckCpuAndMotherboardCompatibility(build.Cpu, build.Motherboard).Errors);
        result.AddErrors(CheckCpuAndRamCompatibility(build.Cpu, build.Rams).Errors);
        result.AddErrors(CheckMotherboardAndRamCompatibility(build.Motherboard, build.Rams).Errors);
        result.AddErrors(CheckCpuAndCoolerCompatibility(build.Cpu, build.Cooler).Errors);
        result.AddErrors(CheckMotherboardAndStorageCompatibility(build.Motherboard, build.Storages).Errors);
        result.AddErrors(CheckMotherboardAndGpuCompatibility(build.Motherboard, build.Gpu).Errors);

        return result;
    }
    
    public bool IsRamCompatible(BuildWithComponents buildWithComponents)
    {
        var cpuRamResult = CheckCpuAndRamCompatibility(buildWithComponents.Cpu, buildWithComponents.Rams);
        if (cpuRamResult.Status == CompatibilityStatus.Incompatible)
            return false;
    
        var motherboardRamResult = CheckMotherboardAndRamCompatibility(buildWithComponents.Motherboard, buildWithComponents.Rams);
        if (motherboardRamResult.Status == CompatibilityStatus.Incompatible)
            return false;

        return true;
    }
}