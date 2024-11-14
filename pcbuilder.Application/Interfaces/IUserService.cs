using pcbuilder.Application.DTOs.Users;
using pcbuilder.Shared;

namespace pcbuilder.Application.Interfaces;

public interface IUserService
{
    public Task<Result<LoginDto>> RegisterUser(string username, string password);
}