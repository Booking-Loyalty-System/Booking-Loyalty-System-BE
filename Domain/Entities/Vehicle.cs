using Domain.Enums;

namespace Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string? LicensePlate { get; set; } = null!;
    public VehicleType Type { get; set; }
    public bool IsPrimary { get; set; }
    public string? VehicleName { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Customer Customer { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
