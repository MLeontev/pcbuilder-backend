using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CoolerService;

public interface ICoolerService
{
    public Task<Result<Cooler>> GetById(int id);

    public Task<Result<PagedList<Cooler>>> Get(string? searchQuery, int page, int pageSize);
    
    public Task<Result<PagedList<Cooler>>> GetCompatible(
        string? searchQuery, int page, int pageSize,
        BuildComponentIds buildComponentIds);
}