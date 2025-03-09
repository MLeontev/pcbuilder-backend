namespace pcbuilder.Domain.Models.Common;

public class BuildComponent
{
    public int BuildId { get; set; }
    public Build Build { get; set; } = null!;

    public int PcComponentId { get; set; }
    public PcComponent PcComponent { get; set; } = null!;

    public int Quantity { get; set; } = 1;
}