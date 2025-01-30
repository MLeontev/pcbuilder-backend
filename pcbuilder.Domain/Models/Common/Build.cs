namespace pcbuilder.Domain.Models.Common;

public class Build
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public List<BuildComponent> BuildComponents { get; set; } = [];
}