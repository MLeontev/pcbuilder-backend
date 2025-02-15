using System.ComponentModel.DataAnnotations.Schema;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Models.Coolers;

[Table("Coolers")]
public class Cooler : PcComponent
{
    public int Tdp { get; set; }

    public int? Height { get; set; }

    public int? WaterCoolingSizeId { get; set; }
    public WaterCoolingSize? WaterCoolingSize { get; set; }

    public List<CoolerSocket> CoolerSockets { get; set; } = [];

    public override string Description =>
        Height.HasValue
            ? $"TDP: {Tdp} Вт, высота: {Height} мм"
            : $"TDP: {Tdp} Вт, размер радиатора: {WaterCoolingSize?.Size}";
}