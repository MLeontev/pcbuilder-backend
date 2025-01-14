using System.ComponentModel.DataAnnotations;

namespace pcbuilder.Domain.Models.Cpus;

public class CpuSeries
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public List<Cpu> Cpus { get; set; } = [];
}