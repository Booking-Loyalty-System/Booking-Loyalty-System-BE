using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/wash-packages")]
public class WashPackageController : ControllerBase
{
    private readonly IWashPackageService _washPackageService;

    public WashPackageController(IWashPackageService washPackageService)
    {
        _washPackageService = washPackageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActive()
    {
        var result = await _washPackageService.GetAllActiveAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _washPackageService.GetByIdAsync(id);
        if (result == null)
            return NotFound(ApiResponse<object>.FailResponse("Wash package not found."));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}
