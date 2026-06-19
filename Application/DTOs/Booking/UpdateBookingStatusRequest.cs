using Domain.Enums;

namespace Application.DTOs.Booking;

public class UpdateBookingStatusRequest
{
    public BookingStatus TargetStatus { get; set; }
    public Guid? StaffId { get; set; } // Dùng cho trạng thái InProgress
    public string? Reason { get; set; } // Dùng cho trạng thái Cancelled / NoShow
}