namespace pcbuilder.Api.Contracts.Builds;

public class CompatibilityErrorResponse
{
    public int Status { get; set; }
    
    public string Message { get; set; } = string.Empty;
}