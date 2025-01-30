using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Models.Cases;

[Table("Cases")]
public class Case : PcComponent
{
    public int MaxGpuLength { get; set; }

    public int MaxCoolerHeight { get; set; }

    public int MaxPsuLength { get; set; }

    public List<CaseWaterCoolingSize> CaseWaterCoolingSizes { get; set; } = [];
    public List<CaseStorageFormFactor> CaseStorageFormFactors { get; set; } = [];
    public List<CaseMotherboardFormFactor> CaseMotherboardFormFactors { get; set; } = [];
}