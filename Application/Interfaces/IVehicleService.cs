using Application.DTOs.Vehicle;

namespace Application.Interfaces;

public interface IVehicleService
{
    Task<VehicleResponse> AddVehicleAsync(Guid userId, AddVehicleRequest request);
    Task<List<VehicleResponse>> GetMyVehiclesAsync(Guid userId);
    Task DeleteVehicleAsync(Guid userId, Guid vehicleId);
    Task<VehicleResponse> UpdateVehicleAsync(Guid userId, Guid vehicleId, UpdateVehicleRequest request);
}
