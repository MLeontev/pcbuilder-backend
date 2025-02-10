using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.Models.Cases;

[Table("Cases")]
public class Case : PcComponent
{
    public int MaxGpuLength { get; set; }

    public int MaxCoolerHeight { get; set; }

    public int MaxPsuLength { get; set; }
    
    public int MaxMotherboardFormFactorId { get; set; }
    public MotherboardFormFactor MaxMotherboardFormFactor { get; set; } = null!;
    
    public int Slots25 { get; set; } 
    public int Slots35 { get; set; }

    public List<CaseWaterCoolingSize> CaseWaterCoolingSizes { get; set; } = [];
}