using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.BuildService;

public class BuildService : IBuildService
{
    private readonly CompatibilityChecker _compatibilityChecker;
    private readonly ICpuRepository _cpuRepository;
    private readonly IMotherboardRepository _motherboardRepository;

    public BuildService(
        CompatibilityChecker compatibilityChecker, 
        ICpuRepository cpuRepository, 
        IMotherboardRepository motherboardRepository)
    {
        _compatibilityChecker = compatibilityChecker;
        _cpuRepository = cpuRepository;
        _motherboardRepository = motherboardRepository;
    }

    public async Task<Result<CompatibilityResult>> CheckBuildCompatibility(CheckBuildCompatibilityDto buildDto)
    {
        Cpu? cpu = null;
        if (buildDto.CpuId.HasValue)
        {
            cpu = await _cpuRepository.GetById(buildDto.CpuId.Value);
            if (cpu == null)
            {
                return Result.Failure<CompatibilityResult>(ComponentErrors.NotFound(buildDto.CpuId.Value));
            }
        }
        
        Motherboard? motherboard = null;
        if (buildDto.MotherboardId.HasValue)
        {
            motherboard = await _motherboardRepository.GetById(buildDto.MotherboardId.Value);
            if (motherboard == null)
            {
                return Result.Failure<CompatibilityResult>(ComponentErrors.NotFound(buildDto.MotherboardId.Value));
            }
        }
        
        var buildWithComponentsDto = new BuildWithComponentsDto
        {
            Cpu = cpu,
            Motherboard = motherboard
        };
        
        var compatibilityResult = _compatibilityChecker.CheckBuildCompatibility(buildWithComponentsDto);
        
        return Result.Success(compatibilityResult);
    }
}