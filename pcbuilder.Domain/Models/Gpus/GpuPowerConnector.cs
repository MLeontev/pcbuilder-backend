using pcbuilder.Domain.Models.PowerSupplies;

namespace pcbuilder.Domain.Models.Gpus;

public class GpuPowerConnector
{
    public int GpuId { get; set; }
    public Gpu Gpu { get; set; } = null!;

    public int PowerConnectorId { get; set; }
    public PowerConnector PowerConnector { get; set; } = null!;
    
    public int Quantity { get; set; }
}