using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.RamService;

public class RamService : IRamService
{
    private readonly IRamRepository _ramRepository;
    private readonly IBuildService _buildService;
    private readonly CompatibilityChecker _compatibilityChecker;

    public RamService(
        IRamRepository ramRepository, 
        IBuildService buildService, 
        CompatibilityChecker compatibilityChecker)
    {
        _ramRepository = ramRepository;
        _buildService = buildService;
        _compatibilityChecker = compatibilityChecker;
    }

    public async Task<Result<Ram>> GetById(int id)
    {
        var ram = await _ramRepository.GetById(id);
        
        return ram == null 
            ? Result.Failure<Ram>(ComponentErrors.NotFound(id)) 
            : Result.Success(ram);
    }

    public async Task<Result<PagedList<Ram>>> Get(string? searchQuery, int page, int pageSize)
    {
        var rams = await _ramRepository.Get(searchQuery, page, pageSize);
        return Result.Success(rams);
    }

    public async Task<Result<PagedList<Ram>>> GetCompatible(
        string? searchQuery, int page, int pageSize,
        BuildComponentIds buildComponentIds)
    {
        var getComponentsResult = await _buildService.GetAllComponents(buildComponentIds);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<PagedList<Ram>>(getComponentsResult.Error);
        }
        
        var build = getComponentsResult.Value;
        build.Rams = [];
        
        var availableRams = await _ramRepository.Get(searchQuery, 1, int.MaxValue);

        var compatibleRams = new List<Ram>();
        
        foreach (var ram in availableRams.Items)
        {
            build.Rams = [ram];
            if (_compatibilityChecker.IsRamCompatible(build))
            {
                compatibleRams.Add(ram);
            }
        }
        
        var pagedCompatibleRams = compatibleRams
            .Skip((page - 1) * pageSize)
            .Take(pageSize)            
            .ToList();

        var pagedResult = new PagedList<Ram>(pagedCompatibleRams, page, pageSize, compatibleRams.Count);

        return Result.Success(pagedResult);
    }
}