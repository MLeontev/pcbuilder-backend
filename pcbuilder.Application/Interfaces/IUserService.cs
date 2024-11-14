using pcbuilder.Application.DTOs.Users;
using pcbuilder.Shared;

namespace pcbuilder.Application.Interfaces;

public interface IUserService
{
    public Task<Result<AuthResult>> RegisterUser(string username, string password);
    
    public Task<Result<AuthResult>> Login(string username, string password);
    
    public Task<Result<AuthResult>> RefreshToken(string accessToken, string refreshToken);
    
    public Task<Result> Logout(string refreshToken);
}