namespace pcbuilder.Api.Contracts.Components;

public class ComponentDetailsResponse
{
    public int Id { get; set; }

    public string? ImagePath { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Dictionary<string, string> Specifications { get; set; } = new();
}