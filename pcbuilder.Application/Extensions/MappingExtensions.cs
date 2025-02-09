using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Models.Storage;

namespace pcbuilder.Application.Extensions;

public static class MappingExtensions
{
    public static BuildDto ToDto(this Build build)
    {
        return new BuildDto
        {
            Id = build.Id,
            Name = build.Name,
            Description = build.Description,
            CreatedAt = build.CreatedAt,
            UpdatedAt = build.UpdatedAt,
            Components = build.BuildComponents.ToDto()
        };
    }
    
    public static BuildComponentIds ToDto(this List<BuildComponent> buildComponents)
    {
        return new BuildComponentIds
        {
            CpuId = buildComponents.FirstOrDefault(bc => bc.PcComponent is Cpu)?.PcComponentId,
            MotherboardId = buildComponents.FirstOrDefault(bc => bc.PcComponent is Motherboard)?.PcComponentId,
            GpuId = buildComponents.FirstOrDefault(bc => bc.PcComponent is Gpu)?.PcComponentId,
            CoolerId = buildComponents.FirstOrDefault(bc => bc.PcComponent is Cooler)?.PcComponentId,
            RamIds = buildComponents
                .Where(bc => bc.PcComponent is Ram)
                .Select(bc => bc.PcComponentId)
                .ToList(),
            StorageIds = buildComponents
                .Where(bc => bc.PcComponent is Storage)
                .Select(bc => bc.PcComponentId)
                .ToList()
        };
    }
    
    public static List<BuildComponent> ToBuildComponents(this BuildWithComponents components)
    {
        var buildComponents = new List<BuildComponent>();

        if (components.Cpu != null)
            buildComponents.Add(new BuildComponent { PcComponentId = components.Cpu.Id });

        if (components.Motherboard != null)
            buildComponents.Add(new BuildComponent { PcComponentId = components.Motherboard.Id });
        
        if (components.Gpu != null)
            buildComponents.Add(new BuildComponent { PcComponentId = components.Gpu.Id });
        
        if (components.Cooler != null)
            buildComponents.Add(new BuildComponent { PcComponentId = components.Cooler.Id });
        
        if (components.Rams != null)
            buildComponents.AddRange(components.Rams.Select(ram => new BuildComponent { PcComponentId = ram.Id }));
        
        if (components.Storages != null)
            buildComponents.AddRange(components.Storages.Select(storage => new BuildComponent { PcComponentId = storage.Id }));

        return buildComponents;
    }
}