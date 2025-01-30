using pcbuilder.Domain.Models.Cpus;

namespace pcbuilder.Domain.Models.Coolers;

public class CoolerSocket
{
    public int CoolerId { get; set; }
    public Cooler Cooler { get; set; } = null!;

    public int SocketId { get; set; }
    public Socket Socket { get; set; } = null!;
}