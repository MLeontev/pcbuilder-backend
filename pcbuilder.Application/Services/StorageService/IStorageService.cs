using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Storage;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.StorageService;

public interface IStorageService
{
    public Task<Result<Storage>> GetById(int id);

    public Task<Result<PagedList<Storage>>> Get(string? searchQuery, int page, int pageSize);
    
    public Task<Result<PagedList<Storage>>> GetCompatible(
        string? searchQuery, int page, int pageSize,
        BuildComponentIds buildComponentIds);
}