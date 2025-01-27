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

    public async Task<int> AddAsync(Build build)
    {
        _dbContext.Builds.Add(build);
        await _dbContext.SaveChangesAsync();
        return build.Id;
    }
}