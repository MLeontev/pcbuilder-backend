using pcbuilder.Application.DTOs.Builds;

namespace pcbuilder.Api.Contracts.Builds;

public class BuildResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string CreatedAt { get; set; } = string.Empty;
    public string? UpdatedAt { get; set; }

    public BuildComponentIds Components { get; set; } = null!;
}