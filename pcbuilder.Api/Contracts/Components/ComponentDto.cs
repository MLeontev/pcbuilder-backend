namespace pcbuilder.Api.Contracts.Components;

public class ComponentDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}