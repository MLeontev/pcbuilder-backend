using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Ram;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class RamRepository : IRamRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RamRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Ram>> Get(string? searchQuery, int page, int pageSize)
    {
        IQueryable<Ram> query = _dbContext.Rams
            .Include(r => r.Brand)
            .Include(r => r.MemoryType);
        
        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(r =>
                (r.Brand.Name + " " + r.Name).ToLower().Contains(searchQuery.ToLower()));
        
        var totalCount = await query.CountAsync();
        
        var rams = await query
            .OrderBy(r => r.Brand.Name)
            .ThenBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<Ram>(rams, page, pageSize, totalCount);
    }

    public async Task<Ram?> GetById(int id)
    {
        return await _dbContext.Rams
            .Include(r => r.Brand)
            .Include(r => r.MemoryType)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}