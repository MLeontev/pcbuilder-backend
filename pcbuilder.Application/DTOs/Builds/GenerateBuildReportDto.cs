using pcbuilder.Domain.DTOs;

namespace pcbuilder.Application.DTOs.Builds;

public class GenerateBuildReportDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public BuildWithComponents Components { get; set; } = null!;
}