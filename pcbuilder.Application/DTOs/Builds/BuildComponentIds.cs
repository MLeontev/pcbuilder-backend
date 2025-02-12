namespace pcbuilder.Application.DTOs.Builds;

public class BuildComponentIds
{
    public int? CpuId { get; set; }

    public int? MotherboardId { get; set; }
    
    public int? GpuId { get; set; }
    
    public int? CoolerId { get; set; }
    
    public List<int>? RamIds { get; set; }
    
    public List<int>? StorageIds { get; set; }
    
    public int? CaseId { get; set; }
    
    public int? PsuId { get; set; }
}