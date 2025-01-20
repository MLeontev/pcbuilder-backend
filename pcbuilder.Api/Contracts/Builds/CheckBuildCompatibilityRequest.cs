namespace pcbuilder.Api.Contracts.Builds;

public class CheckBuildCompatibilityRequest
{
    public int? CpuId { get; set; }
    
    public int? MotherboardId { get; set; }
}