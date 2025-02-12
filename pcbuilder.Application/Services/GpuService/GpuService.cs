using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.GpuService;

public class GpuService : IGpuService
{
    private readonly IGpuRepository _gpuRepository;
    private readonly IBuildService _buildService;
    private readonly CompatibilityChecker _compatibilityChecker;

    public GpuService(
        IGpuRepository gpuRepository, 
        IBuildService buildService, 
        CompatibilityChecker compatibilityChecker)
    {
        _gpuRepository = gpuRepository;
        _buildService = buildService;
        _compatibilityChecker = compatibilityChecker;
    }

    public async Task<Result<Gpu>> GetById(int id)
    {
        var gpu = await _gpuRepository.GetById(id);
        
        return gpu == null
            ? Result.Failure<Gpu>(ComponentErrors.NotFound(id))
            : Result.Success(gpu);
    }

    public async Task<Result<PagedList<Gpu>>> Get(string? searchQuery, int page, int pageSize)
    {
        var gpus = await _gpuRepository.Get(searchQuery, page, pageSize);
        return Result.Success(gpus);
    }

    public async Task<Result<PagedList<Gpu>>> GetCompatible(string? searchQuery, int page, int pageSize, BuildComponentIds buildComponentIds)
    {
        var getComponentsResult = await _buildService.GetAllComponents(buildComponentIds);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<PagedList<Gpu>>(getComponentsResult.Error);
        }
        
        var build = getComponentsResult.Value;
        
        var availableComponents = await _gpuRepository.Get(searchQuery, 1, int.MaxValue);

        var compatibleComponents = new List<Gpu>();
        
        foreach (var component in availableComponents.Items)
        {
            build.Gpu = component;
            if (_compatibilityChecker.IsGpuCompatible(build))
            {
                compatibleComponents.Add(component);
            }
        }
        
        var pagedCompatibleComponents = compatibleComponents
            .Skip((page - 1) * pageSize)
            .Take(pageSize)            
            .ToList();

        var pagedResult = new PagedList<Gpu>(pagedCompatibleComponents, page, pageSize, compatibleComponents.Count);

        return Result.Success(pagedResult);
    }
}