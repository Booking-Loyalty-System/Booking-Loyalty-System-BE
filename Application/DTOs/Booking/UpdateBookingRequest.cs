namespace Application.DTOs.Booking;

public class UpdateBookingRequest
{
    public Guid? BranchId { get; set; }
    public Guid? WashPackageId { get; set; }
    public DateOnly? BookingDate { get; set; }
    public TimeOnly? StartTime { get; set; }
}