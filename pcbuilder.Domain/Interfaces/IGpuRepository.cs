using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Gpus;

namespace pcbuilder.Domain.Interfaces;

public interface IGpuRepository
{
    public Task<PagedList<Gpu>> Get(string? searchQuery, int page, int pageSize);

    public Task<Gpu?> GetById(int id);
}