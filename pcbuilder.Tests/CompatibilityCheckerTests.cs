using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Services;

namespace pcbuilder.Tests;

public class CompatibilityCheckerTests
{
    private readonly CompatibilityChecker _checker = new();

    [Fact]
    public void CheckCpuAndMotherboardCompatibility_ShouldReturnCompatible_WhenSocketsMatch()
    {
        var cpu = new Cpu { SocketId = 1 };
        var motherboard = new Motherboard { SocketId = 1 };

        var result = _checker.CheckCpuAndMotherboardCompatibility(cpu, motherboard);

        Assert.Equal(CompatibilityStatus.Compatible, result.Status);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void CheckCpuAndMotherboardCompatibility_ShouldReturnIncompatible_WhenSocketsMismatch()
    {
        var cpu = new Cpu { SocketId = 1, Socket = new Socket { Name = "AM4" } };
        var motherboard = new Motherboard { SocketId = 2, Socket = new Socket { Name = "LGA1700" } };

        var result = _checker.CheckCpuAndMotherboardCompatibility(cpu, motherboard);

        Assert.Equal(CompatibilityStatus.Incompatible, result.Status);
        Assert.Single(result.Errors);
        Assert.Equal("Cpu.Motherboard.SocketMismatch", result.Errors[0].Code);
    }

    [Fact]
    public void CheckCpuAndRamCompatibility_ShouldReturnCompatible_WhenMemoryTypeAndSpeedAreSupported()
    {
        var cpu = new Cpu
        {
            MaxMemoryCapacity = 64,
            CpuMemories = [new CpuMemory() { MemoryTypeId = 1, MaxMemorySpeed = 3200 }]
        };

        var rams = new List<Ram>
        {
            new()
            {
                MemoryTypeId = 1, Frequency = 3200, Modules = 2, Capacity = 8,
                MemoryType = new MemoryType { Name = "DDR4" }
            }
        };

        var result = _checker.CheckCpuAndRamCompatibility(cpu, rams);

        Assert.Equal(CompatibilityStatus.Compatible, result.Status);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void CheckCpuAndRamCompatibility_ShouldReturnIncompatible_WhenMemoryTypeIsNotSupported()
    {
        var cpu = new Cpu
        {
            MaxMemoryCapacity = 64,
            CpuMemories = [new CpuMemory() { MemoryTypeId = 1, MaxMemorySpeed = 3200 }]
        };

        var rams = new List<Ram>
        {
            new()
            {
                MemoryTypeId = 2, Frequency = 3200, Modules = 2, Capacity = 8,
                Brand = new Brand { Id = 1, Name = "BrandName" },
                MemoryType = new MemoryType { Name = "DDR5" }
            }
        };

        var result = _checker.CheckCpuAndRamCompatibility(cpu, rams);

        Assert.Equal(CompatibilityStatus.Incompatible, result.Status);
        Assert.Single(result.Errors);
        Assert.Equal("Cpu.Ram.RamTypeMismatch", result.Errors[0].Code);
    }

    [Fact]
    public void CheckCpuAndRamCompatibility_ShouldReturnWarning_WhenMemorySpeedExceedsMaxSupported()
    {
        var cpu = new Cpu
        {
            MaxMemoryCapacity = 64,
            CpuMemories = new List<CpuMemory>
            {
                new() { MemoryTypeId = 1, MaxMemorySpeed = 3200 }
            }
        };

        var rams = new List<Ram>
        {
            new()
            {
                MemoryTypeId = 1, Frequency = 3600, Modules = 2, Capacity = 8,
                Brand = new Brand { Id = 1, Name = "BrandName" },
                MemoryType = new MemoryType { Name = "DDR4" }
            }
        };

        var result = _checker.CheckCpuAndRamCompatibility(cpu, rams);

        Assert.Equal(CompatibilityStatus.CompatibleWithLimitations, result.Status);
        Assert.Single(result.Errors);
        Assert.Equal("Cpu.Ram.SpeedExceeded", result.Errors[0].Code);
    }

    [Fact]
    public void CheckCpuAndRamCompatibility_ShouldReturnIncompatible_WhenTotalRamCapacityExceedsLimit()
    {
        var cpu = new Cpu
        {
            MaxMemoryCapacity = 32,
            CpuMemories = [new CpuMemory() { MemoryTypeId = 1, MaxMemorySpeed = 3200 }]
        };

        var rams = new List<Ram>
        {
            new()
            {
                MemoryTypeId = 1, Frequency = 3200, Modules = 2, Capacity = 16,
                MemoryType = new MemoryType { Name = "DDR4" }
            },
            new()
            {
                MemoryTypeId = 1, Frequency = 3200, Modules = 2, Capacity = 16,
                MemoryType = new MemoryType { Name = "DDR4" }
            }
        };

        var result = _checker.CheckCpuAndRamCompatibility(cpu, rams);

        Assert.Equal(CompatibilityStatus.Incompatible, result.Status);
        Assert.Single(result.Errors);
        Assert.Equal("Cpu.Ram.CapacityExceeded", result.Errors[0].Code);
    }
}