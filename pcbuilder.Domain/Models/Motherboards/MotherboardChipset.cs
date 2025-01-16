using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Models.Motherboards;

public class MotherboardChipset
{
    public int Id { get; set; }
    
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
}