namespace pcbuilder.Api.Contracts.Components;

public class ComponentResponse
{
    public int Id { get; set; }
    public string? ImagePath { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}