namespace pcbuilder.Api.Contracts;

public class PagedResponse<T>
{
    public List<T> Items { get; set; } = [];
    
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalCount { get; set; }
    
    public int TotalPages { get; set; }
    
    public bool HasPreviousPage { get; set; }
    
    public bool HasNextPage { get; set; }
}