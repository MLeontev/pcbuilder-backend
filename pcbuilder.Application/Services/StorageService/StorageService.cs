using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Storage;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.StorageService;

public class StorageService : IStorageService
{
    private readonly IStorageRepository _storageRepository;
    private readonly IBuildService _buildService;
    private readonly CompatibilityChecker _compatibilityChecker;

    public StorageService(
        IStorageRepository storageRepository, 
        IBuildService buildService, 
        CompatibilityChecker compatibilityChecker)
    {
        _storageRepository = storageRepository;
        _buildService = buildService;
        _compatibilityChecker = compatibilityChecker;
    }

    public async Task<Result<Storage>> GetById(int id)
    {
        var storage = await _storageRepository.GetById(id);
        
        return storage == null 
            ? Result.Failure<Storage>(ComponentErrors.NotFound(id)) 
            : Result.Success(storage);
    }

    public async Task<Result<PagedList<Storage>>> Get(string? searchQuery, int page, int pageSize)
    {
        var storages = await _storageRepository.Get(searchQuery, page, pageSize);
        return Result.Success(storages);
    }

    public async Task<Result<PagedList<Storage>>> GetCompatible(string? searchQuery, int page, int pageSize, BuildComponentIds buildComponentIds)
    {
        var getComponentsResult = await _buildService.GetAllComponents(buildComponentIds);

        if (getComponentsResult.IsFailure)
        {
            return Result.Failure<PagedList<Storage>>(getComponentsResult.Error);
        }
        
        var build = getComponentsResult.Value;
        build.Storages = [];
        
        var availableComponents = await _storageRepository.Get(searchQuery, 1, int.MaxValue);

        var compatibleComponents = new List<Storage>();
        
        foreach (var component in availableComponents.Items)
        {
            build.Storages = [component];
            if (_compatibilityChecker.IsStorageCompatible(build))
            {
                compatibleComponents.Add(component);
            }
        }
        
        var pagedCompatibleComponents = compatibleComponents
            .Skip((page - 1) * pageSize)
            .Take(pageSize)            
            .ToList();

        var pagedResult = new PagedList<Storage>(pagedCompatibleComponents, page, pageSize, compatibleComponents.Count);

        return Result.Success(pagedResult);
    }
}