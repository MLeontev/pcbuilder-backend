namespace pcbuilder.Application.DTOs.Builds;

public class SaveBuildDto
{
    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public BuildComponentIds Components { get; set; } = null!;
}