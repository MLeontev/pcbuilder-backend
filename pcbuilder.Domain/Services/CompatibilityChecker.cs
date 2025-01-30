using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;

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

        foreach (var ram in rams)
            if (ram.MemoryTypeId != motherboard.MemoryTypeId)
                result.AddError(CompatibilityErrors.MotherboardRamTypeMismatch(motherboard, ram));

        var totalRamModules = rams.Sum(r => r.Modules);
        if (totalRamModules > motherboard.MemorySlots)
            result.AddError(CompatibilityErrors.MotherboardRamSlotLimitExceeded(motherboard, totalRamModules));

        var totalRamCapacity = rams.Sum(ram => ram.TotalCapacity);
        if (totalRamCapacity > motherboard.MaxMemoryCapacity)
            result.AddError(CompatibilityErrors.MotherboardRamCapacityExceeded(motherboard, totalRamCapacity));

        foreach (var ram in rams)
            if (ram.Frequency > motherboard.MaxMemorySpeed)
                result.AddError(CompatibilityErrors.MotherboardRamSpeedExceeded(motherboard, ram));

        return result;
    }

    public CompatibilityResult CheckBuildCompatibility(BuildWithComponentsDto buildWithComponentsDto)
    {
        var result = new CompatibilityResult();

        result.AddErrors(CheckCpuAndMotherboardCompatibility(
            buildWithComponentsDto.Cpu,
            buildWithComponentsDto.Motherboard).Errors);

        result.AddErrors(CheckCpuAndRamCompatibility(
            buildWithComponentsDto.Cpu,
            buildWithComponentsDto.Rams).Errors);

        result.AddErrors(CheckMotherboardAndRamCompatibility(
            buildWithComponentsDto.Motherboard,
            buildWithComponentsDto.Rams).Errors);

        return result;
    }
}