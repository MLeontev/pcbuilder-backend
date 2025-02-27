using pcbuilder.Application.DTOs.Builds;

namespace pcbuilder.Api.Contracts.Builds;

public class GenerateBuildReportRequest
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public BuildComponentIds Components { get; set; } = null!;
}