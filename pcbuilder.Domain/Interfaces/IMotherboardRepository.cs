using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.Interfaces;

public interface IMotherboardRepository
{
    public Task<PagedList<Motherboard>> Get(string? searchQuery, int page, int pageSize);
    
    public Task<Motherboard?> GetById(int id);
}