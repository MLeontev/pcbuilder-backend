using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.Models.Motherboards;

public class MotherboardStorageInterface
{
    public int MotherboardStorageId { get; set; }
    public MotherboardStorage MotherboardStorageSlot { get; set; } = null!;

    public int StorageInterfaceId { get; set; }
    public StorageInterface StorageInterface { get; set; } = null!;
}