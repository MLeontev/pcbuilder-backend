using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cases;

namespace pcbuilder.Domain.Interfaces;

public interface ICaseRepository
{
    public Task<PagedList<Case>> Get(string? searchQuery, int page, int pageSize);

    public Task<Case?> GetById(int id);
}