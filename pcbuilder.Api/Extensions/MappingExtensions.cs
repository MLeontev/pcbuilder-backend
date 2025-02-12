using pcbuilder.Api.Contracts;
using pcbuilder.Api.Contracts.Builds;
using pcbuilder.Api.Contracts.Builds.Compatibility;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cases;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.PowerSupplies;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Models.Storage;
using pcbuilder.Domain.Services;

namespace pcbuilder.Api.Extensions;

public static class MappingExtensions
{
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

    private static ComponentDetailsResponse ToBaseComponentDetailsResponse(this PcComponent component)
    {
        return new ComponentDetailsResponse
        {
            Id = component.Id,
            ImagePath = component.ImagePath,
            Name = component.FullName,
            Description = component.Description
        };
    }

    public static ComponentDetailsResponse ToComponentDetailsResponse(this Cpu cpu)
    {
        var cpuMemories = cpu.CpuMemories
            .OrderBy(cm => cm.MemoryType.Name)
            .Select(cm => $"{cm.MemoryType.Name}-{cm.MaxMemorySpeed}")
            .ToList();

        var response = cpu.ToBaseComponentDetailsResponse();

        response.Specifications = new Dictionary<string, string>
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
        };

        return response;
    }

    public static ComponentDetailsResponse ToComponentDetailsResponse(this Motherboard motherboard)
    {
        var sataPortsCount = motherboard.MotherboardStorages
            .Where(ms => ms.SupportedInterfaces.Any(si => si.StorageInterface.Name == "SATA"))
            .Sum(ms => ms.Quantity);
        
        var m2Slots = motherboard.MotherboardStorages
            .Where(ms => ms.SupportedInterfaces.Any(si => si.StorageInterface.Name.StartsWith("M.2")))
            .ToList();
        
        var m2SlotDescriptions = new List<string>();
        foreach (var slot in m2Slots)
        {
            var formFactorNames = slot.SupportedFormFactors
                .Select(sf => sf.StorageFormFactor.Name.Replace("M.2-", ""))
                .OrderBy(int.Parse)
                .ToList();
            
            var interfaces = slot.SupportedInterfaces
                .Select(si => si.StorageInterface.Name.Replace("M.2 ", ""))
                .OrderBy(si => si)
                .ToList();
            
            var formFactorDescription = string.Join("/", formFactorNames);
            var interfaceDescription = string.Join("/", interfaces);
            
            m2SlotDescriptions.Add($"{slot.Quantity} x {formFactorDescription} ({interfaceDescription})");
        }
        
        var powerConnectors = motherboard.MotherboardPowerConnectors
            .OrderBy(pc => pc.PowerConnector.Pins)
            .Select(pc => $"{pc.Quantity} x {pc.PowerConnector.Name}")
            .ToList();
        
        var response = motherboard.ToBaseComponentDetailsResponse();

        response.Specifications = new Dictionary<string, string>
        {
            { "Чипсет", motherboard.MotherboardChipset.Name },
            { "Сокет", motherboard.Socket.Name },
            { "Форм-фактор", motherboard.FormFactor.Name },
            { "Тип памяти", motherboard.MemoryType.Name },
            { "Максимальный объем памяти", $"{motherboard.MaxMemoryCapacity} ГБ" },
            { "Максимальная частота памяти", $"{motherboard.MaxMemorySpeed} МГц" },
            { "Количество портов SATA", sataPortsCount.ToString() },
            { "Количество разъемов M.2", m2Slots.Count.ToString() },
            { "Разъемы M.2", string.Join(", ", m2SlotDescriptions)},
            { "Количество слотов PCIe", motherboard.PcieSlotsCount.ToString() },
            { "Версия PCIe", motherboard.PcieVersion },
            { "Разъемы питания", string.Join(", ", powerConnectors) }
        };

        return response;
    }

    public static ComponentDetailsResponse ToComponentDetailsResponse(this Ram ram)
    {
        var response = ram.ToBaseComponentDetailsResponse();

        response.Specifications = new Dictionary<string, string>
        {
            { "Тип памяти", ram.MemoryType.Name },
            { "Количество модулей", $"{ram.Modules}" },
            { "Объем одного модуля", $"{ram.Capacity} ГБ" },
            { "Общий объем памяти", $"{ram.TotalCapacity} ГБ" },
            { "Частота", $"{ram.Frequency} МГц" }
        };

        return response;
    }

    public static ComponentDetailsResponse ToComponentDetailsResponse(this Cooler cooler)
    {
        var specifications = new Dictionary<string, string>
        {
            { "TDP", $"{cooler.Tdp} Вт" }
        };

        if (cooler.WaterCoolingSize != null)
        {
            specifications.Add("Размер радиатора", $"{cooler.WaterCoolingSize.Size} мм");
        }
        else
        {
            if (cooler.Height.HasValue) specifications.Add("Высота", $"{cooler.Height} мм");
        }

        var sockets = string.Join(", ",
            cooler.CoolerSockets
                .OrderBy(cs => cs.Socket.Name)
                .Select(cs => $"{cs.Socket.Name}"));
        
        specifications.Add("Совместимые сокеты", sockets);

        var response = cooler.ToBaseComponentDetailsResponse();
        response.Specifications = specifications;
        return response;
    }
    
    public static ComponentDetailsResponse ToComponentDetailsResponse(this Storage storage)
    {
        var response = storage.ToBaseComponentDetailsResponse();

        response.Specifications = new Dictionary<string, string>
        {
            { "Тип накопителя", storage.StorageType.Name },
            { "Интерфейс", storage.StorageInterface.Name },
            { "Форм-фактор", storage.StorageFormFactor.Name },
            { "Объем", $"{storage.Capacity} ГБ" },
            { "Скорость чтения", $"{storage.ReadSpeed} Мбайт/сек" },
            { "Скорость записи", $"{storage.WriteSpeed} Мбайт/сек" }
        };

        return response;
    }

    public static ComponentDetailsResponse ToComponentDetailsResponse(this Gpu gpu)
    {
        var response = gpu.ToBaseComponentDetailsResponse();

        var powerConnectors = gpu.GpuPowerConnectors
            .OrderBy(pc => pc.PowerConnector.Pins)
            .Select(pc => $"{pc.Quantity} x {pc.PowerConnector.Pins}-pin")
            .ToList();
        
        response.Specifications = new Dictionary<string, string>
        {
            { "Графический процессор", gpu.Chipset.Name },
            { "Объем видеопамяти", $"{gpu.MemoryCapacity} ГБ" },
            { "Штатная частота работы видеочипа", $"{gpu.CoreClock} МГц" },
            { "Турбочастота", $"{gpu.BoostClock} МГц" },
            { "Разрядность шины памяти", $"{gpu.BusWidth} бит" },
            { "Интерфейс", $"PCIe {gpu.PcieVersion} x{gpu.PcieLanes}" },
            { "Разъемы дополнительного питания", string.Join(", ", powerConnectors)},
            { "TDP", $"{gpu.Tdp} Вт" },
            { "Длина", $"{gpu.Length} мм" },
            { "Рекомендуемая мощность БП", $"{gpu.RecommendedPsuPower} Вт" }
        };

        return response;
    }
    
    public static ComponentDetailsResponse ToComponentDetailsResponse(this Case pcCase)
    {
        var response = pcCase.ToBaseComponentDetailsResponse();

        var caseWaterCoolingSizes = pcCase.CaseWaterCoolingSizes
            .OrderBy(cws => cws.WaterCoolingSize.Size)
            .Select(cws => $"{cws.WaterCoolingSize.Size} мм")
            .ToList();

        response.Specifications = new Dictionary<string, string>
        {
            { "Максимальная длина видеокарты", $"{pcCase.MaxGpuLength} мм" },
            { "Максимальная высота кулера", $"{pcCase.MaxCoolerHeight} мм" },
            { "Максимальная длина блока питания", $"{pcCase.MaxPsuLength} мм" },
            { "Форм-фактор материнской платы", pcCase.MaxMotherboardFormFactor.Name },
            { "Количество слотов 2.5", $"{pcCase.Slots25}" },
            { "Количество слотов 3.5", $"{pcCase.Slots35}" },
            { "Поддерживаемые размеры водяного охлаждения", string.Join(", ", caseWaterCoolingSizes) }
        };

        return response;
    }
    
    public static ComponentDetailsResponse ToComponentDetailsResponse(this PowerSupply psu)
    {
        var response = psu.ToBaseComponentDetailsResponse();

        var powerConnectors = psu.PsuPowerConnectors
            .OrderBy(pc => pc.PowerConnector.Name)
            .Select(pc => $"{pc.Quantity} x {pc.PowerConnector.Name}")
            .ToList();

        response.Specifications = new Dictionary<string, string>
        {
            { "Сертификат 80 PLUS", psu.PsuEfficiency.Name },
            { "Мощность", $"{psu.Power} Вт" },
            { "Длина", $"{psu.Length} мм" },
            { "Разъемы питания", string.Join(", ", powerConnectors) }
        };

        return response;
    }

    #endregion
}