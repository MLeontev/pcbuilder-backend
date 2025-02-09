using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Extensions;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Models.Storage;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.BuildService;

public class BuildService : IBuildService
{
    private readonly IBuildRepository _buildRepository;
    private readonly CompatibilityChecker _compatibilityChecker;
    private readonly ICpuRepository _cpuRepository;
    private readonly IMotherboardRepository _motherboardRepository;
    private readonly IRamRepository _ramRepository;
    private readonly ICoolerRepository _coolerRepository;
    private readonly IStorageRepository _storageRepository;
    private readonly IGpuRepository _gpuRepository;

    public BuildService(
        CompatibilityChecker compatibilityChecker,
        ICpuRepository cpuRepository,
        IMotherboardRepository motherboardRepository,
        IBuildRepository buildRepository, 
        IRamRepository ramRepository, 
        ICoolerRepository coolerRepository, 
        IStorageRepository storageRepository, 
        IGpuRepository gpuRepository)
    {
        _compatibilityChecker = compatibilityChecker;
        _cpuRepository = cpuRepository;
        _motherboardRepository = motherboardRepository;
        _buildRepository = buildRepository;
        _ramRepository = ramRepository;
        _coolerRepository = coolerRepository;
        _storageRepository = storageRepository;
        _gpuRepository = gpuRepository;
    }

    public async Task<Result<CompatibilityResult>> CheckBuildCompatibility(BuildComponentIds build)
    {
        var getComponentsResult = await GetAllComponents(build);
        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<CompatibilityResult>(getComponentsResult.Error);
        }

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
        var getComponentsResult = await GetAllComponents(saveBuildDto.Components);

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
        if (build == null)
        {
            return Result.Failure(BuildErrors.NotFound(buildId));
        }

        if (build.UserId != userId) return Result.Failure(BuildErrors.ForbiddenAccess);

        await _buildRepository.Delete(build);

        return Result.Success();
    }

    public async Task<Result<BuildWithComponents>> GetAllComponents(BuildComponentIds build)
    {
        Cpu? cpu = null;
        if (build.CpuId.HasValue)
        {
            cpu = await _cpuRepository.GetById(build.CpuId.Value);
            if (cpu == null)
                return Result.Failure<BuildWithComponents>(ComponentErrors.NotFound(build.CpuId.Value));
        }

        Motherboard? motherboard = null;
        if (build.MotherboardId.HasValue)
        {
            motherboard = await _motherboardRepository.GetById(build.MotherboardId.Value);
            if (motherboard == null)
                return Result.Failure<BuildWithComponents>(ComponentErrors.NotFound(build.MotherboardId.Value));
        }
        
        Gpu? gpu = null;
        if (build.MotherboardId.HasValue)
        {
            gpu = await _gpuRepository.GetById(build.GpuId.Value);
            if (gpu == null)
                return Result.Failure<BuildWithComponents>(ComponentErrors.NotFound(build.GpuId.Value));
        }
        
        Cooler? cooler = null;
        if (build.CoolerId.HasValue)
        {
            cooler = await _coolerRepository.GetById(build.CoolerId.Value);
            if (cooler == null)
                return Result.Failure<BuildWithComponents>(ComponentErrors.NotFound(build.CoolerId.Value));
        }
        
        List<Ram> rams = [];
        if (build.RamIds?.Any() == true)
        {
            foreach (var ramId in build.RamIds)
            {
                var ram = await _ramRepository.GetById(ramId);
                if (ram == null)
                    return Result.Failure<BuildWithComponents>(ComponentErrors.NotFound(ramId));

                rams.Add(ram);
            }
        }
        
        List<Storage> storages = [];
        if (build.StorageIds?.Any() == true)
        {
            foreach (var storageId in build.StorageIds)
            {
                var storage = await _storageRepository.GetById(storageId);
                if (storage == null)
                    return Result.Failure<BuildWithComponents>(ComponentErrors.NotFound(storageId));

                storages.Add(storage);
            }
        }

        return Result.Success(new BuildWithComponents
        {
            Cpu = cpu,
            Motherboard = motherboard,
            Gpu = gpu,
            Cooler = cooler,
            Rams = rams,
            Storages = storages
        });
    }
}