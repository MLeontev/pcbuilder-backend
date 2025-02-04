using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.Models.Motherboards;

public class MotherboardStorage
{
    public int Id { get; set; }
    
    public int MotherboardId { get; set; }
    public Motherboard Motherboard { get; set; } = null!;

    public List<MotherboardStorageFormFactor> SupportedFormFactors { get; set; } = [];
    public List<MotherboardStorageInterface> SupportedInterfaces { get; set; } = [];

    public int Quantity { get; set; }
}