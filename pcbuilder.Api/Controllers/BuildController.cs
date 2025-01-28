using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetBuildById(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var result = await _buildService.GetById(id, userId);
        
        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(new GetBuildResponse
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                CreatedAt = result.Value.CreatedAt.ToString("dd.MM.yyyy HH:mm:ss"),
                UpdatedAt = result.Value.UpdatedAt?.ToString("dd.MM.yyyy HH:mm:ss"),
                CpuId = result.Value.CpuId,
                MotherboardId = result.Value.MotherboardId
            });
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SaveBuild(SaveBuildRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _buildService.SaveBuild(new SaveBuildDto
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