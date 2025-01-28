using Microsoft.EntityFrameworkCore;
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

    public async Task<Build?> GetById(int id)
    {
        return await _dbContext.Builds
            .Include(b => b.BuildComponents)
            .ThenInclude(bc => bc.PcComponent)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<int> Add(Build build)
    {
        _dbContext.Builds.Add(build);
        await _dbContext.SaveChangesAsync();
        return build.Id;
    }
}