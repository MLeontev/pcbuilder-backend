using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.PowerSupplies;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.PowerSupplyService;

public class PsuService : IPsuService
{
    private readonly IPsuRepository _psuRepository;
    private readonly IBuildService _buildService;
    private readonly CompatibilityChecker _compatibilityChecker;

    public PsuService(
        IPsuRepository psuRepository, 
        IBuildService buildService, 
        CompatibilityChecker compatibilityChecker)
    {
        _psuRepository = psuRepository;
        _buildService = buildService;
        _compatibilityChecker = compatibilityChecker;
    }
    
    public async Task<Result<PowerSupply>> GetById(int id)
    {
        var powerSupply = await _psuRepository.GetById(id);
        
        return powerSupply == null
            ? Result.Failure<PowerSupply>(ComponentErrors.NotFound(id))
            : Result.Success(powerSupply);
    }

    public async Task<Result<PagedList<PowerSupply>>> Get(string? searchQuery, int page, int pageSize)
    {
        var powerSupplies = await _psuRepository.Get(searchQuery, page, pageSize);
        return Result.Success(powerSupplies);
    }

    public async Task<Result<PagedList<PowerSupply>>> GetCompatible(string? searchQuery, int page, int pageSize, BuildComponentIds buildComponentIds)
    {
        var getComponentsResult = await _buildService.GetAllComponents(buildComponentIds);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<PagedList<PowerSupply>>(getComponentsResult.Error);
        }
        
        var build = getComponentsResult.Value;
        
        var availableComponents = await _psuRepository.Get(searchQuery, 1, int.MaxValue);

        var compatibleComponents = new List<PowerSupply>();
        
        foreach (var component in availableComponents.Items)
        {
            build.Psu = component;
            if (_compatibilityChecker.IsPsuCompatible(build))
            {
                compatibleComponents.Add(component);
            }
        }
        
        var pagedCompatibleComponents = compatibleComponents
            .Skip((page - 1) * pageSize)
            .Take(pageSize)            
            .ToList();

        var pagedResult = new PagedList<PowerSupply>(pagedCompatibleComponents, page, pageSize, compatibleComponents.Count);

        return Result.Success(pagedResult);
    }
}