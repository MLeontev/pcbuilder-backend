using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Cases;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.CaseService;

public interface ICaseService
{
    public Task<Result<Case>> GetById(int id);

    public Task<Result<PagedList<Case>>> Get(string? searchQuery, int page, int pageSize);
    
    public Task<Result<PagedList<Case>>> GetCompatible(
        string? searchQuery, int page, int pageSize,
        BuildComponentIds buildComponentIds);
}