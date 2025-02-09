using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Gpus;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class GpuRepository : IGpuRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GpuRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Gpu>> Get(string? searchQuery, int page, int pageSize)
    {
        IQueryable<Gpu> query = _dbContext.Gpus
            .Include(g => g.Brand)
            .Include(g => g.Chipset)
            .Include(g => g.GpuPowerConnectors)
            .ThenInclude(pc => pc.PowerConnector);

        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(g =>
                (g.Brand.Name + " " + g.Name).ToLower().Contains(searchQuery.ToLower()));
        
        var totalCount = await query.CountAsync();
        
        var gpus = await query
            .OrderBy(g => g.Brand.Name)
            .ThenBy(g => g.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<Gpu>(gpus, page, pageSize, totalCount);
    }

    public async Task<Gpu?> GetById(int id)
    {
        return await _dbContext.Gpus
            .Include(g => g.Brand)
            .Include(g => g.Chipset)
            .Include(g => g.GpuPowerConnectors)
            .ThenInclude(pc => pc.PowerConnector)
            .FirstOrDefaultAsync(gpu => gpu.Id == id);
    }
}