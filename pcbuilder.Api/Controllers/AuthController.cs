using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts;
using pcbuilder.Api.Contracts.Users;
using pcbuilder.Api.Extensions;
using pcbuilder.Application.Interfaces;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<RegisterUserRequest> _registerUserRequestValidator;
    private readonly IValidator<LoginUserRequest> _loginUserRequestValidator;

    public AuthController(
        IUserService userService, 
        IValidator<RegisterUserRequest> registerUserRequestValidator, 
        IValidator<LoginUserRequest> loginUserRequestValidator)
    {
        _userService = userService;
        _registerUserRequestValidator = registerUserRequestValidator;
        _loginUserRequestValidator = loginUserRequestValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var validationResult = await _registerUserRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }
        
        var result = await _userService.RegisterUser(request.Username, request.Password);

        if (result.IsFailure)
        {
            return result.ToErrorResponse();
        }
        
        var loginDto = result.Value;
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(30),
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("RefreshToken", loginDto.RefreshToken, cookieOptions);
        
        return Ok(UserResponse.FromLoginDto(loginDto));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var validationResult = await _loginUserRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }
        
        var result = await _userService.Login(request.Username, request.Password);
        
        if (result.IsFailure)
        {
            return result.ToErrorResponse();
        }
        
        var loginDto = result.Value;
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(30),
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("RefreshToken", loginDto.RefreshToken, cookieOptions);
        
        return Ok(UserResponse.FromLoginDto(loginDto));
    }
}