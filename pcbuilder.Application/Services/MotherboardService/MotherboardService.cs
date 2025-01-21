using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.MotherboardService;

public class MotherboardService : IMotherboardService
{
    private readonly IMotherboardRepository _motherboardRepository;

    public MotherboardService(IMotherboardRepository motherboardRepository)
    {
        _motherboardRepository = motherboardRepository;
    }

    public async Task<Result<Motherboard>> GetById(int id)
    {
        var motherboard = await _motherboardRepository.GetById(id);
        
        return motherboard == null
            ? Result.Failure<Motherboard>(ComponentErrors.NotFound(id))
            : Result.Success(motherboard);
    }

    public async Task<Result<PagedList<Motherboard>>> Get(string searchQuery, int page, int pageSize)
    {
        var motherboards = await _motherboardRepository.Get(searchQuery, page, pageSize);
        return Result.Success(motherboards);
    }
}