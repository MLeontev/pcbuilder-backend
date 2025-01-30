using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Extensions;
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
    private readonly IBuildRepository _buildRepository;
    private readonly CompatibilityChecker _compatibilityChecker;
    private readonly ICpuRepository _cpuRepository;
    private readonly IMotherboardRepository _motherboardRepository;

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

    public async Task<Result<CompatibilityResult>> CheckBuildCompatibility(BuildComponentIdsDto buildDto)
    {
        var getComponentsResult = await GetAllComponents(buildDto);

        if (getComponentsResult.IsFailure) return Result.Failure<CompatibilityResult>(getComponentsResult.Error);

        var buildWithComponentsDto = getComponentsResult.Value;

        var compatibilityResult = _compatibilityChecker.CheckBuildCompatibility(buildWithComponentsDto);

        return Result.Success(compatibilityResult);
    }

    public async Task<Result<PagedList<BuildDto>>> Get(int userId, string? searchQuery, int page, int pageSize)
    {
        var builds = await _buildRepository.Get(userId, searchQuery, page, pageSize);

        var buildDtos = builds.Items.Select(build => build.ToDto()).ToList();

        var pagedBuildDtoList = new PagedList<BuildDto>(buildDtos, page, pageSize, builds.TotalCount);

        return Result.Success(pagedBuildDtoList);
    }

    public async Task<Result<BuildDto>> GetById(int buildId, int userId)
    {
        var build = await _buildRepository.GetById(buildId);
        if (build == null) return Result.Failure<BuildDto>(BuildErrors.NotFound(buildId));

        return build.UserId != userId 
            ? Result.Failure<BuildDto>(BuildErrors.ForbiddenAccess) 
            : Result.Success(build.ToDto());
    }

    public async Task<Result<int>> SaveBuild(SaveBuildDto saveBuildDto)
    {
        var getComponentsResult = await GetAllComponents(new BuildComponentIdsDto
        {
            CpuId = saveBuildDto.Components.CpuId,
            MotherboardId = saveBuildDto.Components.MotherboardId
        });

        if (getComponentsResult.IsFailure) return Result.Failure<int>(getComponentsResult.Error);

        var components = getComponentsResult.Value;
        var buildComponents = components.ToBuildComponents();

        var build = new Build
        {
            Name = saveBuildDto.Name,
            Description = saveBuildDto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = saveBuildDto.UserId,
            BuildComponents = buildComponents
        };

        var saveResult = await _buildRepository.Add(build);

        return Result.Success(saveResult);
    }

    public async Task<Result> DeleteBuild(int buildId, int userId)
    {
        var build = await _buildRepository.GetById(buildId);

        if (build == null) return Result.Failure(BuildErrors.NotFound(buildId));

        if (build.UserId != userId) return Result.Failure(BuildErrors.ForbiddenAccess);

        await _buildRepository.Delete(build);

        return Result.Success();
    }

    private async Task<Result<BuildWithComponentsDto>> GetAllComponents(BuildComponentIdsDto buildDto)
    {
        Cpu? cpu = null;
        if (buildDto.CpuId.HasValue)
        {
            cpu = await _cpuRepository.GetById(buildDto.CpuId.Value);
            if (cpu == null)
                return Result.Failure<BuildWithComponentsDto>(ComponentErrors.NotFound(buildDto.CpuId.Value));
        }

        Motherboard? motherboard = null;
        if (buildDto.MotherboardId.HasValue)
        {
            motherboard = await _motherboardRepository.GetById(buildDto.MotherboardId.Value);
            if (motherboard == null)
                return Result.Failure<BuildWithComponentsDto>(ComponentErrors.NotFound(buildDto.MotherboardId.Value));
        }

        return Result.Success(new BuildWithComponentsDto
        {
            Cpu = cpu,
            Motherboard = motherboard
        });
    }
}