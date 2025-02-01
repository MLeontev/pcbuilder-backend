using pcbuilder.Domain.DTOs;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services.RamService;

public interface IRamService
{
    public Task<Result<Ram>> GetById(int id);

    public Task<Result<PagedList<Ram>>> Get(string? searchQuery, int page, int pageSize);
}