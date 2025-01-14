using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Models.Storage;

[Table("Storage")]
public class Storage : PcComponent
{
    public int Capacity { get; set; }

    public int ReadSpeed { get; set; }

    public int WriteSpeed { get; set; }

    public int StorageTypeId { get; set; }
    public StorageType StorageType { get; set; } = null!;

    public int StorageInterfaceId { get; set; }
    public StorageInterface StorageInterface { get; set; } = null!;

    public int StorageFormFactorId { get; set; }
    public StorageFormFactor StorageFormFactor { get; set; } = null!;
}