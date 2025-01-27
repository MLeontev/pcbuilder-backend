using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Builds;
using pcbuilder.Api.Extensions;
using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Domain.Models.Common;

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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SaveBuild(SaveBuildRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _buildService.SaveBuild(new BuildDto
        {
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            CpuId = request.CpuId,
            MotherboardId = request.MotherboardId
        });
        
        return result.IsFailure 
            ? result.ToErrorResponse() 
            : Ok(result.Value);
    }
    
    [HttpPost("check")]
    public async Task<IActionResult> CheckBuildCompatibility(BuildComponentsRequest request)
    {
        var result = await _buildService.CheckBuildCompatibility(request.ToBuildComponentsDto());
        
        return result.IsFailure 
            ? result.ToErrorResponse() 
            : Ok(result.Value.ToCompatibilityResponse());
    }
}