using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.Interfaces;

public interface IStorageRepository
{
    public Task<PagedList<Storage>> Get(string? searchQuery, int page, int pageSize);

    public Task<Storage?> GetById(int id);
}