using Microsoft.AspNetCore.Identity;
using pcbuilder.Application.DTOs.Users;
using pcbuilder.Application.Interfaces;
using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Models;
using pcbuilder.Shared;

namespace pcbuilder.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    
    public UserService(UserManager<User> userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result<UserRegistrationResult>> RegisterUser(string username, string password)
    {
        var existingUser = await _userManager.FindByNameAsync(username);
        if (existingUser != null)
        {
            return Result.Failure<UserRegistrationResult>(UserErrors.UsernameTaken);
        }
        
        var user = new User { UserName = username };
        
        var result = await _userManager.CreateAsync(user, password);
        
        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            return Result.Failure<UserRegistrationResult>(
                error != null 
                    ? Error.Validation(error.Code, error.Description) 
                    : Error.Failure("UnknownError", "Регистрация не удалась по неизвестной причине"));
        }
        
        await _userManager.AddToRoleAsync(user, "User");
        
        var accessToken = _jwtProvider.GenerateAccessToken(user, new List<string> { "User" });
        var refreshToken = _jwtProvider.GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
        await _userManager.UpdateAsync(user);
        
        var registrationResult = new UserRegistrationResult
        {
            Id = user.Id,
            UserName = user.UserName,
            AccessToken = accessToken
        };

        return Result.Success(registrationResult);
    }
}