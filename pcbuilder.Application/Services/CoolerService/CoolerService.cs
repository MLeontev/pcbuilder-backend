using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CoolerService;

public class CoolerService : ICoolerService
{
    private readonly ICoolerRepository _coolerRepository;

    public CoolerService(ICoolerRepository coolerRepository)
    {
        _coolerRepository = coolerRepository;
    }

    public async Task<Result<Cooler>> GetById(int id)
    {
        var cooler = await _coolerRepository.GetById(id);
        
        return cooler == null
            ? Result.Failure<Cooler>(ComponentErrors.NotFound(id))
            : Result.Success(cooler);
    }

    public async Task<Result<PagedList<Cooler>>> Get(string? searchQuery, int page, int pageSize)
    {
        var coolers = await _coolerRepository.Get(searchQuery, page, pageSize);
        return Result.Success(coolers);
    }
}