using pcbuilder.Domain.Models.Cpus;

namespace pcbuilder.Domain.Interfaces;

public interface ICpuRepository
{
    
    Task<Cpu?> GetById(int id);
}