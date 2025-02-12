using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Cases;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CaseService;

public class CaseService : ICaseService
{
    private readonly ICaseRepository _caseRepository;

    public CaseService(ICaseRepository caseRepository)
    {
        _caseRepository = caseRepository;
    }
    
    public async Task<Result<Case>> GetById(int id)
    {
        var pcCase = await _caseRepository.GetById(id);
        
        return pcCase == null
            ? Result.Failure<Case>(ComponentErrors.NotFound(id))
            : Result.Success(pcCase);
    }

    public async Task<Result<PagedList<Case>>> Get(string? searchQuery, int page, int pageSize)
    {
        var cases = await _caseRepository.Get(searchQuery, page, pageSize);
        return Result.Success(cases);
    }
}