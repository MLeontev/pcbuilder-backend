using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Cases;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class CaseRepository : ICaseRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Case>> Get(string? searchQuery, int page, int pageSize)
    {
        IQueryable<Case> query = _dbContext.Cases
            .Include(c => c.Brand)
            .Include(c => c.MaxMotherboardFormFactor)
            .Include(c => c.CaseWaterCoolingSizes)
            .ThenInclude(c => c.WaterCoolingSize);
        
        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(c =>
                (c.Brand.Name + " " + c.Name).ToLower().Contains(searchQuery.ToLower()));
        
        var totalCount = await query.CountAsync();
        
        var cases = await query
            .OrderBy(c => c.Brand.Name)
            .ThenBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<Case>(cases, page, pageSize, totalCount);
    }

    public async Task<Case?> GetById(int id)
    {
        return await _dbContext.Cases
            .Include(c => c.Brand)
            .Include(c => c.MaxMotherboardFormFactor)
            .Include(c => c.CaseWaterCoolingSizes)
            .ThenInclude(c => c.WaterCoolingSize)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}