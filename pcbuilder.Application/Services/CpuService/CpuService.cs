using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CpuService;

public class CpuService : ICpuService
{
    private readonly ICpuRepository _cpuRepository;
    private readonly IBuildService _buildService;
    private readonly CompatibilityChecker _compatibilityChecker;

    public CpuService(
        ICpuRepository cpuRepository, 
        IBuildService buildService, 
        CompatibilityChecker compatibilityChecker)
    {
        _cpuRepository = cpuRepository;
        _buildService = buildService;
        _compatibilityChecker = compatibilityChecker;
    }

    public async Task<Result<Cpu>> GetById(int id)
    {
        var cpu = await _cpuRepository.GetById(id);

        return cpu == null
            ? Result.Failure<Cpu>(ComponentErrors.NotFound(id))
            : Result.Success(cpu);
    }

    public async Task<Result<PagedList<Cpu>>> Get(string? searchQuery, int page, int pageSize)
    {
        var cpus = await _cpuRepository.Get(searchQuery, page, pageSize);
        return Result.Success(cpus);
    }

    public async Task<Result<PagedList<Cpu>>> GetCompatible(string? searchQuery, int page, int pageSize, BuildComponentIds buildComponentIds)
    {
        var getComponentsResult = await _buildService.GetAllComponents(buildComponentIds);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<PagedList<Cpu>>(getComponentsResult.Error);
        }
        
        var build = getComponentsResult.Value;
        
        var availableComponents = await _cpuRepository.Get(searchQuery, 1, int.MaxValue);

        var compatibleComponents = new List<Cpu>();
        
        foreach (var component in availableComponents.Items)
        {
            build.Cpu = component;
            if (_compatibilityChecker.IsCpuCompatible(build))
            {
                compatibleComponents.Add(component);
            }
        }
        
        var pagedCompatibleComponents = compatibleComponents
            .Skip((page - 1) * pageSize)
            .Take(pageSize)            
            .ToList();

        var pagedResult = new PagedList<Cpu>(pagedCompatibleComponents, page, pageSize, compatibleComponents.Count);

        return Result.Success(pagedResult);
    }
}