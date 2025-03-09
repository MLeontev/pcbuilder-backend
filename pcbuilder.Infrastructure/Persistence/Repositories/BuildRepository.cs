using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class BuildRepository : IBuildRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BuildRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Build>> Get(int userId, string? searchQuery, int page, int pageSize)
    {
        var query = _dbContext.Builds.Where(b => b.UserId == userId);

        if (!string.IsNullOrEmpty(searchQuery))
            query = query.Where(b => b.Name.ToLower().Contains(searchQuery.ToLower()) || 
                                     (b.Description != null &&
                                      b.Description.ToLower().Contains(searchQuery.ToLower())));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(b => b.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<Build>(items, page, pageSize, totalCount);
    }

    public async Task<Build?> GetById(int id)
    {
        return await _dbContext.Builds
            .Include(b => b.BuildComponents)
            .ThenInclude(bc => bc.PcComponent)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Build?> GetByName(int userId, string name)
    {
        return await _dbContext.Builds
            .Where(b => b.UserId == userId && b.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<int> Add(Build build)
    {
        _dbContext.Builds.Add(build);
        await _dbContext.SaveChangesAsync();
        return build.Id;
    }

    public async Task Update(Build build)
    {
        _dbContext.Builds.Update(build);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Build build)
    {
        _dbContext.Builds.Remove(build);
        await _dbContext.SaveChangesAsync();
    }
}