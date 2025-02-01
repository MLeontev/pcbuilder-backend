namespace pcbuilder.Application.DTOs.Builds;

public class BuildComponentIdsDto
{
    public int? CpuId { get; set; }

    public int? MotherboardId { get; set; }
    
    public List<int>? RamIds { get; set; }
}