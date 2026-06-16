namespace Application.DTOs.Booking;

/// <summary>
/// Staff-facing booking projection. Field names/shape mirror the frontend
/// TypeScript <c>BookingResponseData</c> contract exactly. Dates and times are
/// pre-formatted strings (YYYY-MM-DD / HH:mm) so the client gets a stable format.
/// </summary>
public class BookingResponseData
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = null!;
    public Guid VehicleId { get; set; }
    public string? VehicleName { get; set; }
    public string? LicensePlate { get; set; }
    public Guid WashPackageId { get; set; }
    public string? ServiceName { get; set; }
    public Guid BranchId { get; set; }
    public string BookingDate { get; set; } = null!; // YYYY-MM-DD
    public string StartTime { get; set; } = null!;   // HH:mm
    public string Status { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public int? PointsEarned { get; set; }
    public string CreatedAt { get; set; } = null!;
}
