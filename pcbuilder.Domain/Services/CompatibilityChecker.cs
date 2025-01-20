using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.Services;

public class CompatibilityChecker
{
    public CompatibilityResult CheckProcessorAndMotherboardCompatibility(Cpu? cpu, Motherboard? motherboard)
    {
        var result = new CompatibilityResult();
        
        if (cpu == null || motherboard == null) return result;
        
        if (cpu.SocketId == motherboard.SocketId) return result;
        
        result.AddError(CompatibilityErrors.CpuMotherboardSocketMismatch(cpu, motherboard));
        
        return result;
    }

    public CompatibilityResult CheckBuildCompatibility(BuildWithComponentsDto buildWithComponentsDto)
    {
        var result = new CompatibilityResult();
        
        var cpuMotherboardResult = CheckProcessorAndMotherboardCompatibility(buildWithComponentsDto.Cpu, buildWithComponentsDto.Motherboard);
        
        foreach (var error in cpuMotherboardResult.Errors)
        {
            result.AddError(error);
        }
        
        return result;
    }
}