using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.GpuService;

public class GpuService : IGpuService
{
    private readonly IGpuRepository _gpuRepository;

    public GpuService(IGpuRepository gpuRepository)
    {
        _gpuRepository = gpuRepository;
    }

    public async Task<Result<Gpu>> GetById(int id)
    {
        var gpu = await _gpuRepository.GetById(id);
        
        return gpu == null
            ? Result.Failure<Gpu>(ComponentErrors.NotFound(id))
            : Result.Success(gpu);
    }

    public async Task<Result<PagedList<Gpu>>> Get(string? searchQuery, int page, int pageSize)
    {
        var gpus = await _gpuRepository.Get(searchQuery, page, pageSize);
        return Result.Success(gpus);
    }
}