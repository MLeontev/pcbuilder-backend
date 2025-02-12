using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.PowerSupplies;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.PowerSupplyService;

public class PowerSupplyService : IPowerSupplyService
{
    private readonly IPowerSupplyRepository _powerSupplyRepository;

    public PowerSupplyService(IPowerSupplyRepository powerSupplyRepository)
    {
        _powerSupplyRepository = powerSupplyRepository;
    }
    
    public async Task<Result<PowerSupply>> GetById(int id)
    {
        var powerSupply = await _powerSupplyRepository.GetById(id);
        
        return powerSupply == null
            ? Result.Failure<PowerSupply>(ComponentErrors.NotFound(id))
            : Result.Success(powerSupply);
    }

    public async Task<Result<PagedList<PowerSupply>>> Get(string? searchQuery, int page, int pageSize)
    {
        var powerSupplies = await _powerSupplyRepository.Get(searchQuery, page, pageSize);
        return Result.Success(powerSupplies);
    }
}