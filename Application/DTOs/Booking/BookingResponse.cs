namespace Application.DTOs.Booking;

public class BookingResponse
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = null!;
    public string WashPackageName { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? WashBayName { get; set; }
    public string VehiclePlate { get; set; } = null!;
    public string? VehicleName { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    public string? QrData { get; set; }
    public List<BookingAddOnResponse> AddOns { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
