using Domain.Enums;

namespace Application.DTOs.Auth;

public class RegisterRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; } 
    public DateTime? DateOfBirth { get; set; }
    public string? LicensePlate { get; set; }
    public VehicleType VehicleType { get; set; }
}
