using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CpuService;

public interface ICpuService
{
    public Task<Result<Cpu>> GetById(int id);

    public Task<Result<PagedList<Cpu>>> Get(string? searchQuery, int page, int pageSize);
    
    public Task<Result<PagedList<Cpu>>> GetCompatible(
        string? searchQuery, int page, int pageSize,
        BuildComponentIds buildComponentIds);
}