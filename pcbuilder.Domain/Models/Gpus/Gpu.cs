using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.Models.Gpus;

[Table("Gpus")]
public class Gpu : PcComponent
{
    public int MemoryCapacity { get; set; }

    public int BusWidth { get; set; }

    public int CoreClock { get; set; }

    public int BoostClock { get; set; }

    public int PciSlotId { get; set; }
    public PciSlot PciSlot { get; set; } = null!;

    public int Tdp { get; set; }

    public int Length { get; set; }

    public int ChipsetId { get; set; }
    public GpuChipset Chipset { get; set; } = null!;

    public int RecommendedPsuPower { get; set; }

    public List<GpuPowerConnector> GpuPowerConnectors { get; set; } = new List<GpuPowerConnector>();
}