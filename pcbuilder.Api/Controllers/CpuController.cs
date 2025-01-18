using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Extensions;
using pcbuilder.Application.Services.CpuService;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CpuController : ControllerBase
{
    private readonly ICpuService _cpuService;

    public CpuController(ICpuService cpuService)
    {
        _cpuService = cpuService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _cpuService.GetById(id);
        
        return result.IsFailure
            ? result.ToErrorResponse() 
            : Ok(result.Value.ToComponentDetailsResponse());
    }
}