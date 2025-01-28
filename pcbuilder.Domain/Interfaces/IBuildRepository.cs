using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Domain.Interfaces;

public interface IBuildRepository
{
    Task<Build?> GetById(int id);
    
    Task<int> Add(Build build);
}