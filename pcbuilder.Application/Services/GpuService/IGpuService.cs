using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.GpuService;

public interface IGpuService
{
    public Task<Result<Gpu>> GetById(int id);

    public Task<Result<PagedList<Gpu>>> Get(string? searchQuery, int page, int pageSize);
    
    public Task<Result<PagedList<Gpu>>> GetCompatible(
        string? searchQuery, int page, int pageSize,
        BuildComponentIds buildComponentIds);
}