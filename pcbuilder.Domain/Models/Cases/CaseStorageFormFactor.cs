using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.Models.Cases;

public class CaseStorageFormFactor
{
    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public int StorageFormFactorId { get; set; }
    public StorageFormFactor StorageFormFactor { get; set; } = null!;

    public int Quantity { get; set; }
}