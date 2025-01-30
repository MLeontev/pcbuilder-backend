namespace pcbuilder.Api.Contracts.Builds;

public class SaveBuildRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int? CpuId { get; set; }

    public int? MotherboardId { get; set; }
}