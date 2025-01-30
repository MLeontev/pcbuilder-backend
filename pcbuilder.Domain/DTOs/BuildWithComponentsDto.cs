using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;

namespace pcbuilder.Domain.DTOs;

public class BuildWithComponentsDto
{
    public Cpu? Cpu { get; set; }

    public Motherboard? Motherboard { get; set; }

    public List<Ram>? Rams { get; set; }
}