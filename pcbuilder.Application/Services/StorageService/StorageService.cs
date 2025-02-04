using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Storage;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.StorageService;

public class StorageService : IStorageService
{
    private readonly IStorageRepository _storageRepository;

    public StorageService(IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
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
}