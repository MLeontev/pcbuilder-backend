using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.BuildService;

public interface IBuildService
{
    public Task<Result<CompatibilityResult>> CheckBuildCompatibility(BuildComponentsDto buildDto);
    
    public Task<Result<BuildDto>> GetById(int buildId, int userId);
    
    public Task<Result<int>> SaveBuild(SaveBuildDto saveBuildDto);
}