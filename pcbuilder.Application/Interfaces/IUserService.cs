using pcbuilder.Application.DTOs.Users;
using pcbuilder.Shared;

namespace pcbuilder.Application.Interfaces;

public interface IUserService
{
    public Task<Result<UserRegistrationResult>> RegisterUser(string username, string password);
}