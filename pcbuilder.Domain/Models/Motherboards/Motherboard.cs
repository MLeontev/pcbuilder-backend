using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Ram;

namespace pcbuilder.Domain.Models.Motherboards;

[Table("Motherboards")]
public class Motherboard : PcComponent
{
    public int MotherboardChipsetId { get; set; }
    public MotherboardChipset MotherboardChipset { get; set; } = null!;

    public int SocketId { get; set; }
    public Socket Socket { get; set; } = null!;

    public int FormFactorId { get; set; }
    public MotherboardFormFactor FormFactor { get; set; } = null!;

    public int MemoryTypeId { get; set; }
    public MemoryType MemoryType { get; set; } = null!;

    public int MemorySlots { get; set; }
    public int MaxMemoryCapacity { get; set; }
    public int MaxMemorySpeed { get; set; }

    public List<MotherboardPciSlot> MotherboardPciSlots { get; set; } = [];
    public List<MotherboardStorage> MotherboardStorages { get; set; } = [];
    public List<MotherboardPowerConnector> MotherboardPowerConnectors { get; set; } = [];
}