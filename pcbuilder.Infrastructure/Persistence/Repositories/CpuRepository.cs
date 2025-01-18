using Microsoft.EntityFrameworkCore;
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