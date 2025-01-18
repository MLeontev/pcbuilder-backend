using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CpuService;

public interface ICpuService
{
    public Task<Result<Cpu>> GetById(int id);
}