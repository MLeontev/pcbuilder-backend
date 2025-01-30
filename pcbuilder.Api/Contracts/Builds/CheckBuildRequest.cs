namespace pcbuilder.Api.Contracts.Builds;

public class CheckBuildRequest
{
    public int? CpuId { get; set; }

    public int? MotherboardId { get; set; }
}