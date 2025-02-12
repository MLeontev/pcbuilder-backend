using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.PowerSupplies;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class PowerSupplyRepository : IPowerSupplyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PowerSupplyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<PowerSupply>> Get(string? searchQuery, int page, int pageSize)
    {
        IQueryable<PowerSupply> query = _dbContext.PowerSupplies
            .Include(p => p.Brand)
            .Include(p => p.PsuPowerConnectors)
            .ThenInclude(pc => pc.PowerConnector)
            .ThenInclude(pc => pc.CompatibleConnectors)
            .Include(p => p.PsuEfficiency);
        
        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(p =>
                (p.Brand.Name + " " + p.Name).ToLower().Contains(searchQuery.ToLower()));
        
        var totalCount = await query.CountAsync();
        
        var powerSupplies = await query
            .OrderBy(c => c.Brand.Name)
            .ThenBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<PowerSupply>(powerSupplies, totalCount, page, pageSize);
    }

    public async Task<PowerSupply?> GetById(int id)
    {
        return await _dbContext.PowerSupplies
            .Include(p => p.Brand)
            .Include(p => p.PsuPowerConnectors)
            .ThenInclude(pc => pc.PowerConnector)
            .ThenInclude(pc => pc.CompatibleConnectors)
            .Include(p => p.PsuEfficiency)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}