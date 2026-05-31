namespace Application.DTOs.Booking;

public class CreateBookingRequest
{
    public Guid VehicleId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid StoreId { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly StartTime { get; set; }
}
