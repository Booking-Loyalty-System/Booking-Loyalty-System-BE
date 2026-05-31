using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/stores")]
public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    /// <summary>
    /// Get all active stores
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stores = await _storeService.GetAllActiveStoresAsync();
        return Ok(ApiResponse<object>.SuccessResponse(stores));
    }
}
