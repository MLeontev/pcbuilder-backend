namespace pcbuilder.Api.Contracts.Builds;

public class BuildComponentIds
{
    public int? CpuId { get; set; }

    public int? MotherboardId { get; set; }
    
    public List<int>? RamIds { get; set; }
}