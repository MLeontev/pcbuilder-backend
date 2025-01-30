using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Services;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.BuildService;

public interface IBuildService
{
    public Task<Result<CompatibilityResult>> CheckBuildCompatibility(BuildComponentIdsDto buildDto);

    public Task<Result<PagedList<BuildDto>>> Get(int userId, string? searchQuery, int page, int pageSize);

    public Task<Result<BuildDto>> GetById(int buildId, int userId);

    public Task<Result<int>> SaveBuild(SaveBuildDto saveBuildDto);

    public Task<Result> DeleteBuild(int buildId, int userId);
}