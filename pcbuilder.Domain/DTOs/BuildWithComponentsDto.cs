using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.DTOs;

public class BuildWithComponentsDto
{
    public Cpu? Cpu { get; set; }
    
    public Motherboard? Motherboard { get; set; }
}