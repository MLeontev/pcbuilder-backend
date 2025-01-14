using System.ComponentModel.DataAnnotations;

namespace pcbuilder.Domain.Models.Common;

public class Brand
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public List<PcComponent> PcComponents { get; set; } = [];
}