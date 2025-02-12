using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pcbuilder.Application.Services.GpuService;
using pcbuilder.Application.Services.PowerSupplyService;
using pcbuilder.Domain.Services;
using pcbuilder.Infrastructure.Persistence;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IGpuService _gpuService;
    private readonly IPowerSupplyService _psuService;

    public TestController(ApplicationDbContext context, IGpuService gpuService, IPowerSupplyService psuService)
    {
        _context = context;
        _gpuService = gpuService;
        _psuService = psuService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var psuRes = await _psuService.GetById(18);
        var psu = psuRes.Value;
        
        var gpuRes = await _gpuService.GetById(15);
        var gpu = gpuRes.Value;
        
        return Ok(new CompatibilityChecker().CheckPsuAndGpuCompatibility(psu, gpu));
    }
}