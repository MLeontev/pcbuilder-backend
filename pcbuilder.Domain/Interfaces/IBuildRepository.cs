using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Interfaces;

public interface IBuildRepository
{
    Task<int> AddAsync(Build build);
}