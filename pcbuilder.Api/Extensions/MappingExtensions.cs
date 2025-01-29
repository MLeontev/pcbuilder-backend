using pcbuilder.Api.Contracts;
using pcbuilder.Api.Contracts.Builds;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Services;

namespace pcbuilder.Api.Extensions;

public static class MappingExtensions
{
    #region DTO

    public static BuildComponentsDto ToBuildComponentsDto(this CheckBuildRequest request)
    {
        return new BuildComponentsDto
        {
            CpuId = request.CpuId,
            MotherboardId = request.MotherboardId
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
                { "Максимальная частота памяти", $"{motherboard.MaxMemorySpeed} МГц" },
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

    #endregion

    #region Списки комплектующих

    public static PagedResponse<ComponentResponse> ToPagedResponse(this PagedList<Cpu> pagedList)
    {
        var cpuDtos = pagedList.Items.Select(cpu => new ComponentResponse
        {
            Id = cpu.Id,
            ImagePath = cpu.ImagePath,
            FullName = cpu.FullName,
            Description = cpu.Description
        }).ToList();

        return new PagedResponse<ComponentResponse>
        {
            Items = cpuDtos,
            Page = pagedList.Page,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            TotalPages = pagedList.TotalPages,
            HasPreviousPage = pagedList.HasPreviousPage,
            HasNextPage = pagedList.HasNextPage
        };
    }
    
    public static PagedResponse<ComponentResponse> ToPagedResponse(this PagedList<Motherboard> pagedList)
    {
        var motherboardDtos = pagedList.Items.Select(motherboard => new ComponentResponse
        {
            Id = motherboard.Id,
            ImagePath = motherboard.ImagePath,
            FullName = motherboard.FullName,
            Description = motherboard.Description
        }).ToList();

        return new PagedResponse<ComponentResponse>
        {
            Items = motherboardDtos,
            Page = pagedList.Page,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            TotalPages = pagedList.TotalPages,
            HasPreviousPage = pagedList.HasPreviousPage,
            HasNextPage = pagedList.HasNextPage
        };
    }

    #endregion
}