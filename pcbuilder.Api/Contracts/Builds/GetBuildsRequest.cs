namespace pcbuilder.Api.Contracts.Builds;

public class GetBuildsRequest
{
    public string? SearchQuery { get; set; } = string.Empty;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}