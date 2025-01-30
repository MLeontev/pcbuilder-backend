using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cpus;

namespace pcbuilder.Domain.Interfaces;

public interface ICpuRepository
{
    public Task<PagedList<Cpu>> Get(string? searchQuery, int page, int pageSize);

    public Task<Cpu?> GetById(int id);
}