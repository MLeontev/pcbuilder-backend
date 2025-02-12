using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Cases;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CaseService;

public class CaseService : ICaseService
{
    private readonly ICaseRepository _caseRepository;
    private readonly IBuildService _buildService;
    private readonly CompatibilityChecker _compatibilityChecker;

    public CaseService(
        ICaseRepository caseRepository, 
        IBuildService buildService, 
        CompatibilityChecker compatibilityChecker)
    {
        _caseRepository = caseRepository;
        _buildService = buildService;
        _compatibilityChecker = compatibilityChecker;
    }
    
    public async Task<Result<Case>> GetById(int id)
    {
        var pcCase = await _caseRepository.GetById(id);
        
        return pcCase == null
            ? Result.Failure<Case>(ComponentErrors.NotFound(id))
            : Result.Success(pcCase);
    }

    public async Task<Result<PagedList<Case>>> Get(string? searchQuery, int page, int pageSize)
    {
        var cases = await _caseRepository.Get(searchQuery, page, pageSize);
        return Result.Success(cases);
    }

    public async Task<Result<PagedList<Case>>> GetCompatible(string? searchQuery, int page, int pageSize, BuildComponentIds buildComponentIds)
    {
        var getComponentsResult = await _buildService.GetAllComponents(buildComponentIds);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<PagedList<Case>>(getComponentsResult.Error);
        }
        
        var build = getComponentsResult.Value;
        
        var availableComponents = await _caseRepository.Get(searchQuery, 1, int.MaxValue);

        var compatibleComponents = new List<Case>();
        
        foreach (var component in availableComponents.Items)
        {
            build.Case = component;
            if (_compatibilityChecker.IsCaseCompatible(build))
            {
                compatibleComponents.Add(component);
            }
        }
        
        var pagedCompatibleComponents = compatibleComponents
            .Skip((page - 1) * pageSize)
            .Take(pageSize)            
            .ToList();

        var pagedResult = new PagedList<Case>(pagedCompatibleComponents, page, pageSize, compatibleComponents.Count);

        return Result.Success(pagedResult);
    }
}