using pcbuilder.Domain.Models.Coolers;

namespace pcbuilder.Domain.Models.Cases;

public class CaseWaterCoolingSize
{
    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public int WaterCoolingSizeId { get; set; }
    public WaterCoolingSize WaterCoolingSize { get; set; } = null!;
}