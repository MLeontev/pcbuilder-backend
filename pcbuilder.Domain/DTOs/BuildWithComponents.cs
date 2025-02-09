using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Domain.DTOs;

public class BuildWithComponents
{
    public Cpu? Cpu { get; set; }
    
    public Motherboard? Motherboard { get; set; }
    
    public Gpu? Gpu { get; set; }
    
    public Cooler? Cooler { get; set; }

    public List<Ram>? Rams { get; set; }
    
    public List<Storage>? Storages { get; set; }
}