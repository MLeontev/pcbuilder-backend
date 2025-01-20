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
    
    public Task<PagedList<Motherboard>> Get(string searchQuery, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<Motherboard?> GetById(int id)
    {
        return _dbContext.Motherboards
            .Include(m => m.Brand)
            .Include(m => m.MotherboardChipset)
            .Include(m => m.Socket)
            .Include(m => m.FormFactor)
            .Include(m => m.MemoryType)
            .Include(m => m.MotherboardPciSlots)
                .ThenInclude(mps => mps.PciSlot)
            .Include(m => m.MotherboardStorages)
                .ThenInclude(ms => ms.StorageInterface)
            .Include(m => m.MotherboardStorages)
                .ThenInclude(ms => ms.StorageFormFactor)
            .Include(m => m.MotherboardPowerConnectors)
                .ThenInclude(mpc => mpc.PowerConnector)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}