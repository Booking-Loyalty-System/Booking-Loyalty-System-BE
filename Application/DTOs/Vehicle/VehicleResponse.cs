namespace Application.DTOs.Vehicle;

public class VehicleResponse
{
    public Guid Id { get; set; }
    public string LicensePlate { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public string? VehicleName { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
}
