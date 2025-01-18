using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Components.Cpus;
using pcbuilder.Api.Extensions;
using pcbuilder.Api.Validators.Components;
using pcbuilder.Application.Services.CpuService;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CpuController : ControllerBase
{
    private readonly ICpuService _cpuService;
    private readonly PagedRequestValidator _pagedRequestValidator;

    public CpuController(ICpuService cpuService, PagedRequestValidator pagedRequestValidator)
    {
        _cpuService = cpuService;
        _pagedRequestValidator = pagedRequestValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagedRequest request)
    {
        var validationResult = await _pagedRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }
        
        var result = await _cpuService.Get(request.SearchQuery, request.Page, request.PageSize);
        
        return result.IsFailure
            ? result.ToErrorResponse() 
            : Ok(result.Value.ToPagedResponse());
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