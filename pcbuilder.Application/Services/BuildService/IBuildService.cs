using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.BuildService;

public interface IBuildService
{
    public Task<Result<CompatibilityResult>> CheckBuildCompatibility(CheckBuildCompatibilityDto buildDto);
}