using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.Models.Motherboards;

public class MotherboardStorage
{
    public int MotherboardId { get; set; }
    public Motherboard Motherboard { get; set; } = null!;

    public int StorageInterfaceId { get; set; }
    public StorageInterface StorageInterface { get; set; } = null!;

    public int StorageFormFactorId { get; set; }
    public StorageFormFactor StorageFormFactor { get; set; } = null!;

    public int Quantity { get; set; }
}