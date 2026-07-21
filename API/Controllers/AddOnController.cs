using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/add-ons")]
public class AddOnController : ControllerBase
{
    private readonly IAddOnService _addOnService;

    public AddOnController(IAddOnService addOnService)
    {
        _addOnService = addOnService;
    }

    /// <summary>Public catalog of active add-ons customers can attach to a booking.</summary>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _addOnService.GetAllAsync(activeOnly: true);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}
