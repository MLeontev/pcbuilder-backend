using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Builds;
using pcbuilder.Api.Extensions;
using pcbuilder.Api.Validators.Builds;
using pcbuilder.Application.Services.BuildService;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildController : ControllerBase
{
    private readonly IBuildService _buildService;
    private readonly GetBuildsRequestValidator _getBuildsRequestValidator;
    private readonly SaveBuildRequestValidator _saveBuildRequestValidator;

    public BuildController(
        IBuildService buildService, 
        GetBuildsRequestValidator getBuildsRequestValidator, 
        SaveBuildRequestValidator saveBuildRequestValidator)
    {
        _buildService = buildService;
        _getBuildsRequestValidator = getBuildsRequestValidator;
        _saveBuildRequestValidator = saveBuildRequestValidator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] GetBuildsRequest request)
    {
        var validationResult = await _getBuildsRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }
        
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _buildService.Get(userId, request.SearchQuery, request.Page, request.PageSize);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToPagedResponse());
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetBuildById(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _buildService.GetById(id, userId);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToBuildResponse());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Save(SaveBuildRequest request)
    {
        var validationResult = await _saveBuildRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }
        
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _buildService.SaveBuild(request.ToSaveBuildDto(userId));

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _buildService.DeleteBuild(id, userId);

        return result.IsFailure
            ? result.ToErrorResponse()
            : NoContent();
    }

    [HttpPost("check")]
    public async Task<IActionResult> CheckBuildCompatibility(CheckBuildRequest request)
    {
        var result = await _buildService.CheckBuildCompatibility(request.Components);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToCompatibilityResponse());
    }
}