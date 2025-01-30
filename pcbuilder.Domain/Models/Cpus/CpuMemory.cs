using pcbuilder.Domain.Models.Ram;

namespace pcbuilder.Domain.Models.Cpus;

public class CpuMemory
{
    public int CpuId { get; set; }
    public Cpu Cpu { get; set; } = null!;

    public int MemoryTypeId { get; set; }
    public MemoryType MemoryType { get; set; } = null!;

    public int MaxMemorySpeed { get; set; }
}