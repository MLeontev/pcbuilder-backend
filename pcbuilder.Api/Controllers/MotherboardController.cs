using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _motherboardService.GetById(id);
        
        return result.IsFailure 
            ? result.ToErrorResponse() 
            : Ok(result.Value.ToComponentDetailsResponse());
    }
}