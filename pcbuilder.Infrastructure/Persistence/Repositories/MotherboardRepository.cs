using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Infrastructure.Persistence.Repositories;

public class MotherboardRepository : IMotherboardRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MotherboardRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Motherboard>> Get(string? searchQuery, int page, int pageSize)
    {
        IQueryable<Motherboard> query = _dbContext.Motherboards
            .Include(m => m.Brand)
            .Include(m => m.MotherboardChipset)
            .Include(m => m.Socket)
            .Include(m => m.FormFactor)
            .Include(m => m.MemoryType)
            
            .Include(m => m.MotherboardStorages)
            .ThenInclude(ms => ms.SupportedInterfaces)
            .ThenInclude(si => si.StorageInterface)
            
            .Include(m => m.MotherboardStorages)
            .ThenInclude(ms => ms.SupportedFormFactors)
            .ThenInclude(sf => sf.StorageFormFactor)
            
            .Include(m => m.MotherboardPowerConnectors)
            .ThenInclude(mpc => mpc.PowerConnector);

        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(m =>
                (m.Brand.Name + " " + m.Name).ToLower().Contains(searchQuery.ToLower()));

        var totalCount = await query.CountAsync();

        var motherboards = await query
            .OrderBy(m => m.Brand.Name)
            .ThenBy(m => m.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<Motherboard>(motherboards, page, pageSize, totalCount);
    }

    public Task<Motherboard?> GetById(int id)
    {
        return _dbContext.Motherboards
            .Include(m => m.Brand)
            .Include(m => m.MotherboardChipset)
            .Include(m => m.Socket)
            .Include(m => m.FormFactor)
            .Include(m => m.MemoryType)
            
            .Include(m => m.MotherboardStorages)
            .ThenInclude(ms => ms.SupportedInterfaces)
            .ThenInclude(si => si.StorageInterface)
            
            .Include(m => m.MotherboardStorages)
            .ThenInclude(ms => ms.SupportedFormFactors)
            .ThenInclude(sf => sf.StorageFormFactor)
            
            .Include(m => m.MotherboardPowerConnectors)
            .ThenInclude(mpc => mpc.PowerConnector)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}