using pcbuilder.Api.Contracts;
using pcbuilder.Api.Contracts.Builds;
using pcbuilder.Api.Contracts.Builds.Compatibility;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Services;

namespace pcbuilder.Api.Extensions;

public static class MappingExtensions
{
    #region DTO
    
    public static BuildListItemResponse ToBuildListItemResponse(this BuildDto buildDto)
    {
        return new BuildListItemResponse
        {
            Id = buildDto.Id,
            Name = buildDto.Name,
            Description = buildDto.Description,
            CreatedAt = buildDto.CreatedAt.ToString("dd.MM.yyyy HH:mm:ss"),
            UpdatedAt = buildDto.UpdatedAt.ToString("dd.MM.yyyy HH:mm:ss")
        };
    }
    
    public static PagedResponse<BuildListItemResponse> ToPagedResponse(this PagedList<BuildDto> pagedList)
    {
        return new PagedResponse<BuildListItemResponse>
        {
            Items = pagedList.Items.Select(b => b.ToBuildListItemResponse()).ToList(),
            Page = pagedList.Page,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            TotalPages = pagedList.TotalPages,
            HasPreviousPage = pagedList.HasPreviousPage,
            HasNextPage = pagedList.HasNextPage
        };
    }
    
    public static BuildResponse ToBuildResponse(this BuildDto buildDto)
    {
        return new BuildResponse
        {
            Id = buildDto.Id,
            Name = buildDto.Name,
            Description = buildDto.Description,
            CreatedAt = buildDto.CreatedAt.ToString("dd.MM.yyyy HH:mm:ss"),
            UpdatedAt = buildDto.UpdatedAt.ToString("dd.MM.yyyy HH:mm:ss"),
            Components = buildDto.Components
        };
    }
    
    public static SaveBuildDto ToSaveBuildDto(this SaveBuildRequest request, int userId)
    {
        return new SaveBuildDto
        {
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            Components = request.Components
        };
    }

    public static CompatibilityResponse ToCompatibilityResponse(this CompatibilityResult result)
    {
        return new CompatibilityResponse
        {
            Status = (int)result.Status,
            Errors = result.Errors.Select(error => new CompatibilityErrorResponse
            {
                Status = (int)error.Status,
                Message = error.Message
            }).ToList()
        };
    }
    
    #endregion

    #region Комплектующие с характеристиками

    public static ComponentDetailsResponse ToComponentDetailsResponse(this Cpu cpu)
    {
        var cpuMemories = cpu.CpuMemories
            .OrderBy(cm => cm.MemoryType.Name)
            .Select(cm => $"{cm.MemoryType.Name}-{cm.MaxMemorySpeed}")
            .ToList();

        return new ComponentDetailsResponse
        {
            Id = cpu.Id,
            ImagePath = cpu.ImagePath,
            Name = cpu.FullName,
            Description = cpu.Description,
            Specifications = new Dictionary<string, string>
            {
                { "Серия", cpu.Series.Name },
                { "Разъём", cpu.Socket.Name },
                { "Количество ядер", cpu.Cores.ToString() },
                { "Количество потоков", cpu.Threads.ToString() },
                { "Базовая частота", $"{cpu.BaseClock} ГГц" },
                { "Частота в турбо режиме", $"{cpu.BoostClock} ГГц" },
                { "Тепловыделение", $"{cpu.Tdp} Вт" },
                { "Интегрированная графика", cpu.IntegratedGpu ? "Есть" : "Нет" },
                { "Максимально поддерживаемый объем памяти", $"{cpu.MaxMemoryCapacity} ГБ" },
                { "Поддерживаемая память", string.Join(", ", cpuMemories) }
            }
        };
    }

    public static ComponentDetailsResponse ToComponentDetailsResponse(this Motherboard motherboard)
    {
        // var pciSlots = motherboard.MotherboardPciSlots
        //     .OrderBy(mps => mps.PciSlot.Version)
        //     .Select(mps => $"{mps.PciSlot.Version} - {mps.PciSlot.Bandwidth} Гбит/с")
        //     .ToList();
        //
        // var storages = motherboard.MotherboardStorages
        //     .Select(ms => $"{ms.StorageInterface.Name} - {ms.StorageFormFactor.Name}")
        //     .ToList();

        return new ComponentDetailsResponse
        {
            Id = motherboard.Id,
            ImagePath = motherboard.ImagePath,
            Name = motherboard.FullName,
            Description = motherboard.Description,
            Specifications = new Dictionary<string, string>
            {
                { "Чипсет", motherboard.MotherboardChipset.Name },
                { "Сокет", motherboard.Socket.Name },
                { "Форм-фактор", motherboard.FormFactor.Name },
                { "Тип памяти", motherboard.MemoryType.Name },
                { "Максимальный объем памяти", $"{motherboard.MaxMemoryCapacity} ГБ" },
                { "Максимальная частота памяти", $"{motherboard.MaxMemorySpeed} МГц" }
                // { "Слотов PCIe x16", $""},
                // { "Слотов PCIe x8", $""},
                // { "Слотов PCIe x4", $""},
                // { "Слотов PCIe x1", $""},
                // { "Количество разъемов M.2", $""},
                // { "Разъемы M.2", $""},
                // { "Количество портов SATA", $""},
            }
        };
    }
    
    public static ComponentDetailsResponse ToComponentDetailsResponse(this Ram ram)
    {
        return new ComponentDetailsResponse
        {
            Id = ram.Id,
            ImagePath = ram.ImagePath,
            Name = ram.FullName,
            Description = ram.Description,
            Specifications = new Dictionary<string, string>
            {
                { "Тип памяти", ram.MemoryType.Name },
                { "Объем одного модуля", $"{ram.Capacity} ГБ" },
                { "Количество модулей", $"{ram.Modules}" },
                { "Общий объем памяти", $"{ram.TotalCapacity} ГБ" },
                { "Частота", $"{ram.Frequency} МГц" }
            }
        };
    }

    public static ComponentDetailsResponse ToComponentDetailsResponse(this Cooler cooler)
    {
        var specifications = new Dictionary<string, string>
        {
            {"TDP", $"{cooler.Tdp} Вт"}
        };

        if (cooler.WaterCoolingSize != null)
        {
            specifications.Add("Размер радиатора", $"{cooler.WaterCoolingSize.Size} мм");
        }
        else
        {
            if (cooler.Height.HasValue)
            {
                specifications.Add("Высота", $"{cooler.Height} мм");
            }
        }
        
        var sockets = string.Join(", ", cooler.CoolerSockets.Select(cs => $"{cs.Socket.Name}"));
        specifications.Add("Совместимые сокеты", sockets);
        
        return new ComponentDetailsResponse
        {
            Id = cooler.Id,
            ImagePath = cooler.ImagePath,
            Name = cooler.FullName,
            Description = cooler.Description,
            Specifications = specifications
        };
    }

    #endregion
    
    public static PagedResponse<ComponentResponse> ToPagedResponse<T>(this PagedList<T> pagedList) where T : PcComponent
    {
        var componentDtos = pagedList.Items.Select(component => new ComponentResponse
        {
            Id = component.Id,
            ImagePath = component.ImagePath,
            FullName = component.FullName,
            Description = component.Description
        }).ToList();

        return new PagedResponse<ComponentResponse>
        {
            Items = componentDtos,
            Page = pagedList.Page,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            TotalPages = pagedList.TotalPages,
            HasPreviousPage = pagedList.HasPreviousPage,
            HasNextPage = pagedList.HasNextPage
        };
    }
}