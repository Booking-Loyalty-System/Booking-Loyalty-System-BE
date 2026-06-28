using System.Security.Claims;
using Application.Common;
using Application.DTOs.Vehicle;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPost]
    public async Task<IActionResult> AddVehicle(
        [FromBody] AddVehicleRequest request,
        [FromServices] IValidator<AddVehicleRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var userId = GetUserId();
        var result = await _vehicleService.AddVehicleAsync(userId, request);
        return CreatedAtAction(nameof(GetMyVehicles), null, ApiResponse<object>.SuccessResponse(result, "Vehicle added successfully."));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyVehicles()
    {
        var userId = GetUserId();
        var result = await _vehicleService.GetMyVehiclesAsync(userId);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateVehicle(
        Guid id,
        [FromBody] UpdateVehicleRequest request,
        [FromServices] IValidator<UpdateVehicleRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var userId = GetUserId();
        var result = await _vehicleService.UpdateVehicleAsync(userId, id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Vehicle updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var userId = GetUserId();
        await _vehicleService.DeleteVehicleAsync(userId, id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Vehicle deleted successfully."));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
