namespace pcbuilder.Domain.Models.Motherboards;

public class PciSlot
{
    public int Id { get; set; }
    
    public string Version { get; set; } = string.Empty;
    
    public int Bandwidth { get; set; }
}