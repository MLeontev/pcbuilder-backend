using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.PowerSupplies;

namespace pcbuilder.Domain.Interfaces;

public interface IPowerSupplyRepository
{
    public Task<PagedList<PowerSupply>> Get(string? searchQuery, int page, int pageSize);

    public Task<PowerSupply?> GetById(int id);
}