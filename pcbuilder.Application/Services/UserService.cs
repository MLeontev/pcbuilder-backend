using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    
    public async Task<Result<AuthResult>> RegisterUser(string username, string password)
    {
        var existingUser = await _userManager.FindByNameAsync(username);
        if (existingUser != null)
        {
            return Result.Failure<AuthResult>(UserErrors.UsernameTaken);
        }
        
        var user = new User { UserName = username };
        
        var result = await _userManager.CreateAsync(user, password);
        
        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            return Result.Failure<AuthResult>(
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
        
        var roles = await _userManager.GetRolesAsync(user);
    
        var registrationResult = new AuthResult
        {
            Id = user.Id,
            UserName = user.UserName,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Roles = roles.ToList()
        };

        return Result.Success(registrationResult);
    }

    public async Task<Result<AuthResult>> Login(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return Result.Failure<AuthResult>(UserErrors.InvalidCredentials);
        }
        
        var passwordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!passwordValid)
        {
            return Result.Failure<AuthResult>(UserErrors.InvalidCredentials);
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var accessToken = _jwtProvider.GenerateAccessToken(user, roles);
        var refreshToken = _jwtProvider.GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
        await _userManager.UpdateAsync(user);
        
        var loginResult = new AuthResult
        {
            Id = user.Id,
            UserName = user.UserName!,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Roles = roles.ToList()
        };

        return Result.Success(loginResult);
    }

    public async Task<Result<AuthResult>> RefreshToken(string refreshToken)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);

        if (user == null
            || user.RefreshToken != refreshToken
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Result.Failure<AuthResult>(UserErrors.InvalidToken);
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var newAccessToken = _jwtProvider.GenerateAccessToken(user, roles);
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();
        
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
        await _userManager.UpdateAsync(user);
        
        var refreshResult = new AuthResult
        {
            Id = user.Id,
            UserName = user.UserName!,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            Roles = roles.ToList()
        };

        return Result.Success(refreshResult);
    }

    public async Task<Result> Logout(string refreshToken)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        
        if (user == null)
        {
            return Result.Failure(UserErrors.InvalidToken);
        }
        
        user.RefreshToken = string.Empty;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);
        
        return Result.Success();
    }
}