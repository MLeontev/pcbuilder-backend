using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Api.Extensions;
using pcbuilder.Api.Validators.Components;
using pcbuilder.Application.Services.CaseService;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseController : ControllerBase
{
    private readonly ICaseService _caseService;
    private readonly GetComponentsRequestValidator _getComponentsRequestValidator;

    public CaseController(
        ICaseService caseService, 
        GetComponentsRequestValidator getComponentsRequestValidator)
    {
        _caseService = caseService;
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

        var result = await _caseService.Get(request.SearchQuery, request.Page, request.PageSize);
        
        return result.IsFailure 
            ? result.ToErrorResponse()
            : Ok(result.Value.ToPagedResponse());
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _caseService.GetById(id);
        
        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToComponentDetailsResponse());
    }
}