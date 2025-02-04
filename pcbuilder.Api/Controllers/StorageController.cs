using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Api.Extensions;
using pcbuilder.Api.Validators.Components;
using pcbuilder.Application.Services.StorageService;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly GetComponentsRequestValidator _getComponentsRequestValidator;
    private readonly IStorageService _storageService;

    public StorageController(
        GetComponentsRequestValidator getComponentsRequestValidator, 
        IStorageService storageService)
    {
        _getComponentsRequestValidator = getComponentsRequestValidator;
        _storageService = storageService;
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
        
        var result = await _storageService.Get(request.SearchQuery, request.Page, request.PageSize);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToPagedResponse());
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _storageService.GetById(id);

        return result.IsFailure
            ? result.ToErrorResponse()
            : Ok(result.Value.ToComponentDetailsResponse());
    }
}