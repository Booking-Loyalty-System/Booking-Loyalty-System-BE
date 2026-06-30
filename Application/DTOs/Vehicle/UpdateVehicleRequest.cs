namespace Application.DTOs.Vehicle;

public class UpdateVehicleRequest
{
    public string? LicensePlate { get; set; }
    public string? VehicleType { get; set; }
    public string? VehicleName { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public bool? IsPrimary { get; set; }
}
