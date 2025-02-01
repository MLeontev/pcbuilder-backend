using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Api.Extensions;
using pcbuilder.Api.Validators.Components;
using pcbuilder.Application.Services.RamService;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RamController : ControllerBase
{
    private readonly GetComponentsRequestValidator _getComponentsRequestValidator;
    private readonly IRamService _ramService;

    public RamController(GetComponentsRequestValidator getComponentsRequestValidator, IRamService ramService)
    {
        _getComponentsRequestValidator = getComponentsRequestValidator;
        _ramService = ramService;
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
        
        var result = await _ramService.Get(request.SearchQuery, request.Page, request.PageSize);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToPagedResponse());
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _ramService.GetById(id);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToComponentDetailsResponse());
    }
}