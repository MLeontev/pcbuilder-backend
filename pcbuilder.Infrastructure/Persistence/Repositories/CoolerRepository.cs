using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Coolers;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class CoolerRepository : ICoolerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CoolerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Cooler>> Get(string? searchQuery, int page, int pageSize)
    {
        IQueryable<Cooler> query = _dbContext.Coolers
            .Include(c => c.Brand)
            .Include(c => c.CoolerSockets)
            .ThenInclude(cs => cs.Socket)
            .Include(c => c.WaterCoolingSize);
        
        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(c =>
                (c.Brand.Name + " " + c.Name).ToLower().Contains(searchQuery.ToLower()));
        
        var totalCount = await query.CountAsync();
        
        var coolers = await query
            .OrderBy(c => c.Brand.Name)
            .ThenBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<Cooler>(coolers, page, pageSize, totalCount);
    }

    public async Task<Cooler?> GetById(int id)
    {
        return await _dbContext.Coolers
            .Include(c => c.Brand)
             .Include(c => c.CoolerSockets)
             .ThenInclude(cs => cs.Socket)
            .Include(c => c.WaterCoolingSize)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}