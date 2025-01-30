namespace pcbuilder.Api.Contracts.Builds;

public class GetBuildResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string CreatedAt { get; set; } = string.Empty;
    public string? UpdatedAt { get; set; }

    public int? CpuId { get; set; }

    public int? MotherboardId { get; set; }
}