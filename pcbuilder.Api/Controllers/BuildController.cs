using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts;
using pcbuilder.Api.Contracts.Builds;
using pcbuilder.Api.Extensions;
using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildController : ControllerBase
{
    private readonly IBuildService _buildService;

    public BuildController(IBuildService buildService)
    {
        _buildService = buildService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] GetBuildsRequest request)
    {
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
            : Ok(result.Value.ToGetBuildResponse());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Save(SaveBuildRequest request)
    {
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
        var result = await _buildService.CheckBuildCompatibility(request.ToBuildComponentIdsDto());

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToCompatibilityResponse());
    }
}