using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Interfaces;

public interface IBuildRepository
{
    public Task<PagedList<Build>> Get(int userId, string? searchQuery, int page, int pageSize);

    public Task<Build?> GetById(int id);
    
    public Task<Build?> GetByName(int userId, string name);

    public Task<int> Add(Build build);
    
    public Task Update(Build build);

    public Task Delete(Build build);
}