namespace Application.DTOs.Vehicle;

public class AddVehicleRequest
{
    public string LicensePlate { get; set; } = null!;
    public string VehicleType { get; set; } = null!;
    public string? VehicleName { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public bool IsPrimary { get; set; }
}
