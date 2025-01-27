using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Cpus;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class CpuRepository : ICpuRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CpuRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Cpu>> Get(string? searchQuery, int page, int pageSize)
    {
        IQueryable<Cpu> query = _dbContext.Cpus
            .Include(c => c.Series)
            .Include(c => c.Socket)
            .Include(c => c.Brand)
            .Include(c => c.CpuMemories)
            .ThenInclude(cm => cm.MemoryType);

        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(c =>
                (c.Brand.Name + " " + c.Name).ToLower().Contains(searchQuery.ToLower()));

        var totalCount = await query.CountAsync();

        var cpus = await query
            .OrderBy(c => c.Brand.Name)
            .ThenBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<Cpu>(cpus, page, pageSize, totalCount);
    }
    
    public async Task<Cpu?> GetById(int id)
    {
        return await _dbContext.Cpus
            .Include(c => c.Series)
            .Include(c => c.Socket)
            .Include(c => c.Brand)
            .Include(c => c.CpuMemories)
            .ThenInclude(cm => cm.MemoryType)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}