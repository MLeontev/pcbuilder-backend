namespace pcbuilder.Api.Contracts.Components;

public class PagedRequest
{
    public string SearchQuery { get; set; } = string.Empty;
    
    public int Page { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
}