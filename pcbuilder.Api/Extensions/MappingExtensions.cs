using pcbuilder.Api.Contracts.Components;
using pcbuilder.Domain.Models.Cpus;

namespace pcbuilder.Api.Extensions;

public static class MappingExtensions
{
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
}