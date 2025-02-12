using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.MotherboardService;

public class MotherboardService : IMotherboardService
{
    private readonly IMotherboardRepository _motherboardRepository;
    private readonly IBuildService _buildService;
    private readonly CompatibilityChecker _compatibilityChecker;

    public MotherboardService(
        IMotherboardRepository motherboardRepository, 
        IBuildService buildService, 
        CompatibilityChecker compatibilityChecker)
    {
        _motherboardRepository = motherboardRepository;
        _buildService = buildService;
        _compatibilityChecker = compatibilityChecker;
    }

    public async Task<Result<Motherboard>> GetById(int id)
    {
        var motherboard = await _motherboardRepository.GetById(id);

        return motherboard == null
            ? Result.Failure<Motherboard>(ComponentErrors.NotFound(id))
            : Result.Success(motherboard);
    }

    public async Task<Result<PagedList<Motherboard>>> Get(string? searchQuery, int page, int pageSize)
    {
        var motherboards = await _motherboardRepository.Get(searchQuery, page, pageSize);
        return Result.Success(motherboards);
    }

    public async Task<Result<PagedList<Motherboard>>> GetCompatible(string? searchQuery, int page, int pageSize, BuildComponentIds buildComponentIds)
    {
        var getComponentsResult = await _buildService.GetAllComponents(buildComponentIds);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<PagedList<Motherboard>>(getComponentsResult.Error);
        }
        
        var build = getComponentsResult.Value;
        
        var availableComponents = await _motherboardRepository.Get(searchQuery, 1, int.MaxValue);

        var compatibleComponents = new List<Motherboard>();
        
        foreach (var component in availableComponents.Items)
        {
            build.Motherboard = component;
            if (_compatibilityChecker.IsMotherboardCompatible(build))
            {
                compatibleComponents.Add(component);
            }
        }
        
        var pagedCompatibleComponents = compatibleComponents
            .Skip((page - 1) * pageSize)
            .Take(pageSize)            
            .ToList();

        var pagedResult = new PagedList<Motherboard>(pagedCompatibleComponents, page, pageSize, compatibleComponents.Count);

        return Result.Success(pagedResult);
    }
}