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

    public string PcieVersion { get; set; } = string.Empty;
    public int PcieLanes { get; set; }

    public int Tdp { get; set; }

    public int Length { get; set; }

    public int ChipsetId { get; set; }
    public GpuChipset Chipset { get; set; } = null!;

    public int RecommendedPsuPower { get; set; }

    public List<GpuPowerConnector> GpuPowerConnectors { get; set; } = [];

    public override string Description => $"PCIe {PcieVersion}, {MemoryCapacity} Гб, {BusWidth} бит, {CoreClock} МГц";
}