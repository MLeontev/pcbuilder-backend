using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CpuService;

public class CpuService : ICpuService
{
    private readonly ICpuRepository _cpuRepository;

    public CpuService(ICpuRepository cpuRepository)
    {
        _cpuRepository = cpuRepository;
    }

    public async Task<Result<Cpu>> GetById(int id)
    {
        var cpu = await _cpuRepository.GetById(id);
        
        return cpu == null
            ? Result.Failure<Cpu>(ComponentErrors.NotFound(id))
            : Result.Success(cpu);
    }
}