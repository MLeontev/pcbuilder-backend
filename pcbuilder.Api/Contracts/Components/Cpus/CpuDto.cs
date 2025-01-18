namespace pcbuilder.Api.Contracts.Components.Cpus;

public class CpuDto
{
    public int Id { get; set; }
    
    public string FullName { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
}