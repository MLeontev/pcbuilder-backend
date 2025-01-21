using Microsoft.AspNetCore.Mvc;
using pcbuilder.Api.Contracts.Components;
using pcbuilder.Api.Extensions;
using pcbuilder.Api.Validators.Components;
using pcbuilder.Application.Services.MotherboardService;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotherboardController : ControllerBase
{
    private readonly IMotherboardService _motherboardService;
    private readonly PagedRequestValidator _pagedRequestValidator;

    public MotherboardController(PagedRequestValidator pagedRequestValidator, IMotherboardService motherboardService)
    {
        _pagedRequestValidator = pagedRequestValidator;
        _motherboardService = motherboardService;
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
        
        var result = await _motherboardService.Get(request.SearchQuery, request.Page, request.PageSize);
        
        return result.IsFailure
            ? result.ToErrorResponse() 
            : Ok(result.Value.ToPagedResponse());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _motherboardService.GetById(id);
        
        return result.IsFailure 
            ? result.ToErrorResponse() 
            : Ok(result.Value.ToComponentDetailsResponse());
    }
}