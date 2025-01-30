namespace pcbuilder.Api.Contracts.Builds;

public class CheckBuildRequest
{
    public BuildComponentIds Components { get; set; } = null!;
}