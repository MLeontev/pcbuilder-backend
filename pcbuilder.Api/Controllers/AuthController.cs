using FluentValidation;
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

    public AuthController(
        IUserService userService, 
        IValidator<RegisterUserRequest> registerUserRequestValidator)
    {
        _userService = userService;
        _registerUserRequestValidator = registerUserRequestValidator;
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
        
        return result.IsFailure 
            ? result.ToErrorResponse()
            : Ok(result.Value);
    } 
}