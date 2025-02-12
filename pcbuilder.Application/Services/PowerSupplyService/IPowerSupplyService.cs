using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.PowerSupplies;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.PowerSupplyService;

public interface IPowerSupplyService
{
    public Task<Result<PowerSupply>> GetById(int id);

    public Task<Result<PagedList<PowerSupply>>> Get(string? searchQuery, int page, int pageSize);
}