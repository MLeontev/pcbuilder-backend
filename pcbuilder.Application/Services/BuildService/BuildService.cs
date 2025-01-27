using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Common;
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
    private readonly IBuildRepository _buildRepository;

    public BuildService(
        CompatibilityChecker compatibilityChecker, 
        ICpuRepository cpuRepository, 
        IMotherboardRepository motherboardRepository, 
        IBuildRepository buildRepository)
    {
        _compatibilityChecker = compatibilityChecker;
        _cpuRepository = cpuRepository;
        _motherboardRepository = motherboardRepository;
        _buildRepository = buildRepository;
    }

    public async Task<Result<CompatibilityResult>> CheckBuildCompatibility(BuildComponentsDto buildDto)
    {
        var getComponentsResult = await GetAllComponents(buildDto);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<CompatibilityResult>(getComponentsResult.Error);
        }
        
        var buildWithComponentsDto = getComponentsResult.Value;
        
        var compatibilityResult = _compatibilityChecker.CheckBuildCompatibility(buildWithComponentsDto);
        
        return Result.Success(compatibilityResult);
    }

    public async Task<Result<int>> SaveBuild(BuildDto buildDto)
    {
        var getComponentsResult = await GetAllComponents(new BuildComponentsDto
        {
            CpuId = buildDto.CpuId,
            MotherboardId = buildDto.MotherboardId,
        });

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<int>(getComponentsResult.Error);
        }
        
        var components = getComponentsResult.Value;
        var buildComponents = MapComponentsToBuild(components);
        
        var build = new Build
        {
            Name = buildDto.Name,
            Description = buildDto.Description,
            CreatedAt = DateTime.UtcNow,
            UserId = buildDto.UserId,
            BuildComponents = buildComponents
        };

        var saveResult = await _buildRepository.AddAsync(build);

        return Result.Success(saveResult);
    }

    private async Task<Result<BuildWithComponentsDto>> GetAllComponents(BuildComponentsDto buildDto)
    {
        Cpu? cpu = null;
        if (buildDto.CpuId.HasValue)
        {
            cpu = await _cpuRepository.GetById(buildDto.CpuId.Value);
            if (cpu == null)
            {
                return Result.Failure<BuildWithComponentsDto>(ComponentErrors.NotFound(buildDto.CpuId.Value));
            }
        }
        
        Motherboard? motherboard = null;
        if (buildDto.MotherboardId.HasValue)
        {
            motherboard = await _motherboardRepository.GetById(buildDto.MotherboardId.Value);
            if (motherboard == null)
            {
                return Result.Failure<BuildWithComponentsDto>(ComponentErrors.NotFound(buildDto.MotherboardId.Value));
            }
        }

        return Result.Success(new BuildWithComponentsDto
        {
            Cpu = cpu,
            Motherboard = motherboard
        });
    }
    
    private List<BuildComponent> MapComponentsToBuild(BuildWithComponentsDto components)
    {
        var buildComponents = new List<BuildComponent>();

        if (components.Cpu != null)
        {
            buildComponents.Add(new BuildComponent { PcComponentId = components.Cpu.Id });
        }

        if (components.Motherboard != null)
        {
            buildComponents.Add(new BuildComponent { PcComponentId = components.Motherboard.Id });
        }

        return buildComponents;
    }
}