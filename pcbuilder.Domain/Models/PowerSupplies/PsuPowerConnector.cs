namespace pcbuilder.Domain.Models.PowerSupplies;

public class PsuPowerConnector
{
    public int PsuId { get; set; }
    public PowerSupply PowerSupply { get; set; } = null!;

    public int PowerConnectorId { get; set; }
    public PowerConnector PowerConnector { get; set; } = null!;

    public int Quantity { get; set; }
}