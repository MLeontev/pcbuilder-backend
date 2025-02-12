namespace pcbuilder.Domain.Models.PowerSupplies;

public class PowerConnector
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Pins { get; set; }

    public List<PowerConnectorCompatibility> CompatibleConnectors { get; set; } = [];
}