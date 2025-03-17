using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;
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
}