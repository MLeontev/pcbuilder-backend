namespace pcbuilder.Api.Contracts.Builds;

public class SaveBuildRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public BuildComponentIds Components { get; set; } = null!;
}