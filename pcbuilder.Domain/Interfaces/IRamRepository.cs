using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Ram;

namespace pcbuilder.Domain.Interfaces;

public interface IRamRepository
{
    public Task<PagedList<Ram>> Get(string? searchQuery, int page, int pageSize);

    public Task<Ram?> GetById(int id);
}