using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.BuildService;

public interface IBuildService
{
    public Task<Result<CompatibilityResult>> CheckBuildCompatibility(BuildComponentIds build);

    public Task<Result<PagedList<BuildDto>>> Get(int userId, string? searchQuery, int page, int pageSize);

    public Task<Result<BuildDto>> GetById(int buildId, int userId);

    public Task<Result<int>> SaveBuild(SaveUpdateBuildDto saveBuildDto);
    
    public Task<Result> UpdateBuild(int buildId, int userId, SaveUpdateBuildDto updateBuildDto);

    public Task<Result> DeleteBuild(int buildId, int userId);

    public Task<Result<BuildWithComponents>> GetAllComponents(BuildComponentIds build);
}