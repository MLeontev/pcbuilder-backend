namespace pcbuilder.Application.DTOs.Builds;

public class BuildDto
{
    public int UserId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public int? CpuId { get; set; }
    
    public int? MotherboardId { get; set; }
}