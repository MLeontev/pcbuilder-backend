using System.ComponentModel.DataAnnotations;

namespace pcbuilder.Domain.Models.Common;

public class PcComponent
{
    public int Id { get; set; }
    
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
    
    public string? ImagePath { get; set; }
    
    //public string FullName => $"{Brand.Name} {Name}";
}