namespace pcbuilder.Api.Contracts.Builds;

public class CompatibilityResponse
{
    public int Status { get; set; }

    public List<CompatibilityErrorResponse> Errors { get; set; } = [];
}