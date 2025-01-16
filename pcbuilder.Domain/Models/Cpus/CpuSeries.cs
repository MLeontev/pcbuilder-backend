using System.ComponentModel.DataAnnotations;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Models.Cpus;

public class CpuSeries
{
    public int Id { get; set; }
    
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
}