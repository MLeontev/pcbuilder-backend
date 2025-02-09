using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Api.Extensions;
using pcbuilder.Api.Validators.Components;
using pcbuilder.Application.Services.GpuService;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GpuController: ControllerBase
{
    private readonly IGpuService _gpuService;
    private readonly GetComponentsRequestValidator _getComponentsRequestValidator;

    public GpuController(
        IGpuService gpuService, 
        GetComponentsRequestValidator getComponentsRequestValidator)
    {
        _gpuService = gpuService;
        _getComponentsRequestValidator = getComponentsRequestValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetComponentsRequest request)
    {
        var validationResult = await _getComponentsRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }

        var result = await _gpuService.Get(request.SearchQuery, request.Page, request.PageSize);
        
        return result.IsFailure 
            ? result.ToErrorResponse()
            : Ok(result.Value.ToPagedResponse());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _gpuService.GetById(id);
        
        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToComponentDetailsResponse());
    }
}