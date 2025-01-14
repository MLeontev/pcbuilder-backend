namespace pcbuilder.Domain.Models.Motherboards;

public class MotherboardPciSlot
{
    public int MotherboardId { get; set; }
    public Motherboard Motherboard { get; set; } = null!;
    
    public int PciSlotId { get; set; }
    public PciSlot PciSlot { get; set; } = null!;
    
    public int Quantity { get; set; }
}