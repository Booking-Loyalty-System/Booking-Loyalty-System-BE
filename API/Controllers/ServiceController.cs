using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/services")]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    /// <summary>
    /// Get all active services
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var services = await _serviceService.GetAllActiveServicesAsync();
        return Ok(ApiResponse<object>.SuccessResponse(services));
    }
}
