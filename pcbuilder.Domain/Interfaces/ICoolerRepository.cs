using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Coolers;

namespace pcbuilder.Domain.Interfaces;

public interface ICoolerRepository
{
    public Task<PagedList<Cooler>> Get(string? searchQuery, int page, int pageSize);

    public Task<Cooler?> GetById(int id);
}