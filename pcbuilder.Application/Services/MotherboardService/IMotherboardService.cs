using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.MotherboardService;

public interface IMotherboardService
{
    public Task<Result<Motherboard>> GetById(int id);

    public Task<Result<PagedList<Motherboard>>> Get(string? searchQuery, int page, int pageSize);
}