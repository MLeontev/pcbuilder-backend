using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.RamService;

public class RamService : IRamService
{
    private readonly IRamRepository _ramRepository;

    public RamService(IRamRepository ramRepository)
    {
        _ramRepository = ramRepository;
    }

    public async Task<Result<Ram>> GetById(int id)
    {
        var ram = await _ramRepository.GetById(id);
        
        return ram == null 
            ? Result.Failure<Ram>(ComponentErrors.NotFound(id)) 
            : Result.Success(ram);
    }

    public async Task<Result<PagedList<Ram>>> Get(string? searchQuery, int page, int pageSize)
    {
        var rams = await _ramRepository.Get(searchQuery, page, pageSize);
        return Result.Success(rams);
    }
}