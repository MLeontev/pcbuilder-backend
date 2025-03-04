namespace pcbuilder.Application.DTOs.Builds;

public class BuildDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public BuildComponentIds Components { get; set; } = null!;
}