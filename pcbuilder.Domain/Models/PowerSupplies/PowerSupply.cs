using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Models.PowerSupplies;

[Table("PowerSupplies")]
public class PowerSupply : PcComponent
{
    public int PsuEfficiencyId { get; set; }
    public PsuEfficiency PsuEfficiency { get; set; } = null!;

    public int Power { get; set; }
    
    public int Length { get; set; }

    public List<PsuPowerConnector> PsuPowerConnectors { get; set; } = [];

    public override string Description =>
        $"{Power} Вт, 80+ {PsuEfficiency.Name}, {Length} мм";
}