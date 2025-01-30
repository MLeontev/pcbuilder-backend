namespace pcbuilder.Api.Contracts.Builds;

public class BuildListItemResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string CreatedAt { get; set; }

    public string UpdatedAt { get; set; }
}