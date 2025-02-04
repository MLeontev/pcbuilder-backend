using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class StorageRepository : IStorageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StorageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Storage>> Get(string? searchQuery, int page, int pageSize)
    {
        IQueryable<Storage> query = _dbContext.Storage
            .Include(s => s.Brand)
            .Include(s => s.StorageType)
            .Include(s => s.StorageInterface)
            .Include(s => s.StorageFormFactor);
        
        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(s =>
                (s.Brand.Name + " " + s.Name).ToLower().Contains(searchQuery.ToLower()));
        
        var totalCount = await query.CountAsync();
        
        var storage = await query
            .OrderBy(s => s.Brand.Name)
            .ThenBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<Storage>(storage, page, pageSize, totalCount);
    }

    public async Task<Storage?> GetById(int id)
    {
        return await _dbContext.Storage
            .Include(s => s.Brand)
            .Include(s => s.StorageType)
            .Include(s => s.StorageInterface)
            .Include(s => s.StorageFormFactor)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}