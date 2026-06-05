using Domain.Enums;

namespace Domain.Entities;

public class TimeSlot
{
    public Guid Id { get; set; }
    public Guid WashBayId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeSlotStatus Status { get; set; } = TimeSlotStatus.Available;
    public Guid? BookingId { get; set; }

    // Navigation
    public WashBay WashBay { get; set; } = null!;
    public Booking? Booking { get; set; }
}
