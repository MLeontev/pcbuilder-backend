using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CoolerService;

public class CoolerService : ICoolerService
{
    private readonly ICoolerRepository _coolerRepository;
    private readonly IBuildService _buildService;
    private readonly CompatibilityChecker _compatibilityChecker;

    public CoolerService(
        ICoolerRepository coolerRepository, 
        IBuildService buildService, 
        CompatibilityChecker compatibilityChecker)
    {
        _coolerRepository = coolerRepository;
        _buildService = buildService;
        _compatibilityChecker = compatibilityChecker;
    }

    public async Task<Result<Cooler>> GetById(int id)
    {
        var cooler = await _coolerRepository.GetById(id);
        
        return cooler == null
            ? Result.Failure<Cooler>(ComponentErrors.NotFound(id))
            : Result.Success(cooler);
    }

    public async Task<Result<PagedList<Cooler>>> Get(string? searchQuery, int page, int pageSize)
    {
        var coolers = await _coolerRepository.Get(searchQuery, page, pageSize);
        return Result.Success(coolers);
    }

    public async Task<Result<PagedList<Cooler>>> GetCompatible(string? searchQuery, int page, int pageSize, BuildComponentIds buildComponentIds)
    {
        var getComponentsResult = await _buildService.GetAllComponents(buildComponentIds);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<PagedList<Cooler>>(getComponentsResult.Error);
        }
        
        var build = getComponentsResult.Value;
        
        var availableComponents = await _coolerRepository.Get(searchQuery, 1, int.MaxValue);

        var compatibleComponents = new List<Cooler>();
        
        foreach (var component in availableComponents.Items)
        {
            build.Cooler = component;
            if (_compatibilityChecker.IsCoolerCompatible(build))
            {
                compatibleComponents.Add(component);
            }
        }
        
        var pagedCompatibleComponents = compatibleComponents
            .Skip((page - 1) * pageSize)
            .Take(pageSize)            
            .ToList();

        var pagedResult = new PagedList<Cooler>(pagedCompatibleComponents, page, pageSize, compatibleComponents.Count);

        return Result.Success(pagedResult);
    }
}