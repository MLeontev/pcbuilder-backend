namespace pcbuilder.Domain.Models.PowerSupplies;

public class PowerConnectorCompatibility
{
    public int SourceConnectorId { get; set; }
    public PowerConnector SourceConnector { get; set; } = null!;

    public int CompatibleConnectorId { get; set; }
    public PowerConnector CompatibleConnector { get; set; } = null!;

    public int RequiredQuantity { get; set; }
}