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
    private readonly SaveUpdateBuildRequestValidator _saveUpdateBuildRequestValidator;

    public BuildController(
        IBuildService buildService, 
        GetBuildsRequestValidator getBuildsRequestValidator, 
        SaveUpdateBuildRequestValidator saveUpdateBuildRequestValidator)
    {
        _buildService = buildService;
        _getBuildsRequestValidator = getBuildsRequestValidator;
        _saveUpdateBuildRequestValidator = saveUpdateBuildRequestValidator;
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
    public async Task<IActionResult> Save(SaveUpdateBuildRequest request)
    {
        var validationResult = await _saveUpdateBuildRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }
        
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _buildService.SaveBuild(request.ToSaveUpdateBuildDto(userId));

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, SaveUpdateBuildRequest request)
    {
        var validationResult = await _saveUpdateBuildRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }
        
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _buildService.UpdateBuild(id, userId, request.ToSaveUpdateBuildDto(userId));

        return result.IsFailure
            ? result.ToErrorResponse()
            : NoContent();
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
    public async Task<IActionResult> CheckCompatibility(CheckBuildRequest request)
    {
        var result = await _buildService.CheckBuildCompatibility(request.Components);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToCompatibilityResponse());
    }
}