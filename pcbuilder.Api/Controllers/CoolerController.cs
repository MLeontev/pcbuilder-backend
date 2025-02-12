using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Api.Extensions;
using pcbuilder.Api.Validators.Components;
using pcbuilder.Application.DTOs.Builds;
using pcbuilder.Application.Services.CoolerService;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoolerController : ControllerBase
{
    private readonly ICoolerService _coolerService;
    private readonly GetComponentsRequestValidator _getComponentsRequestValidator;

    public CoolerController(
        ICoolerService coolerService, 
        GetComponentsRequestValidator getComponentsRequestValidator)
    {
        _coolerService = coolerService;
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

        var result = await _coolerService.Get(request.SearchQuery, request.Page, request.PageSize);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToPagedResponse());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _coolerService.GetById(id);
        
        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToComponentDetailsResponse()); 
    }
    
    [HttpGet("compatible")]
    public async Task<IActionResult> GetCompatible([FromQuery] GetComponentsRequest request, [FromQuery] BuildComponentIds buildComponentIds)
    {
        var validationResult = await _getComponentsRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorResponse = validationResult.ToValidationErrorResponse();
            return BadRequest(errorResponse);
        }

        var result = await _coolerService.GetCompatible(request.SearchQuery, request.Page, request.PageSize, buildComponentIds);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToPagedResponse());
    }
}