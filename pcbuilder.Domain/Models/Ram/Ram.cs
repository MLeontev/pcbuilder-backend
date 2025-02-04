using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Models.Ram;

[Table("Rams")]
public class Ram : PcComponent
{
    public int MemoryTypeId { get; set; }
    public MemoryType MemoryType { get; set; } = null!;

    public int Modules { get; set; }

    public int Capacity { get; set; }

    public int Frequency { get; set; }
    
    public int TotalCapacity => Modules * Capacity;

    public override string Description => $"{MemoryType.Name} {Capacity} ГБx{Modules} шт, {Frequency} МГц";
}