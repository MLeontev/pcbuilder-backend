using pcbuilder.Domain.Models.PowerSupplies;

namespace pcbuilder.Domain.Models.Motherboards;

public class MotherboardPowerConnector
{
    public int MotherboardId { get; set; }
    public Motherboard Motherboard { get; set; } = null!;

    public int PowerConnectorId { get; set; }
    public PowerConnector PowerConnector { get; set; } = null!;

    public int Quantity { get; set; }
}