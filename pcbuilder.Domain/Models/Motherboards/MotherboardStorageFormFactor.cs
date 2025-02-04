using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.Models.Motherboards;

public class MotherboardStorageFormFactor
{
    public int MotherboardStorageId { get; set; }
    public MotherboardStorage MotherboardStorageSlot { get; set; } = null!;

    public int StorageFormFactorId { get; set; }
    public StorageFormFactor StorageFormFactor { get; set; } = null!;
}