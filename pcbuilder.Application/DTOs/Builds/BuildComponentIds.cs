namespace pcbuilder.Application.DTOs.Builds;

public class BuildComponentIds
{
    public int? CpuId { get; set; }

    public int? MotherboardId { get; set; }
    
    public int? CoolerId { get; set; }
    
    public List<int>? RamIds { get; set; }
    
    public List<int>? StorageIds { get; set; }
}