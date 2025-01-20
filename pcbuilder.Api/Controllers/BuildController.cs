using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Builds;
using pcbuilder.Api.Extensions;
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

    [HttpPost("check")]
    public async Task<IActionResult> CheckBuildCompatibility(CheckBuildCompatibilityRequest request)
    {
        var result = await _buildService.CheckBuildCompatibility(request.ToCheckBuildCompatibilityDto());
        
        return result.IsFailure 
            ? result.ToErrorResponse() 
            : Ok(result.Value.ToCompatibilityResponse());
    }
}