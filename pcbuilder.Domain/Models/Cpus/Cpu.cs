using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Models.Cpus;

[Table("Cpus")]
public class Cpu : PcComponent
{
    public int SeriesId { get; set; }
    public CpuSeries Series { get; set; } = null!;
    
    public int SocketId { get; set; }
    public Socket Socket { get; set; } = null!;
    
    public int Cores { get; set; }
    public int Threads { get; set; }
    
    public decimal BaseClock { get; set; }
    public decimal BoostClock { get; set; }
    
    public int Tdp { get; set; }
    
    public bool IntegratedGpu { get; set; }
    
    public int MaxMemoryCapacity { get; set; }
    
    public List<CpuMemory> CpuMemories { get; set; } = [];

    public override string Description => $"{Socket.Name}, {Cores} x {BaseClock} ГГц, TDP {Tdp}";
}
