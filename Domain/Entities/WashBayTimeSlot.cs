using Domain.Enums;

namespace Domain.Entities;

public class WashBayTimeSlot
{
    public Guid Id { get; set; }
    public Guid WashBayId { get; set; }
    public Guid TimeSlotId { get; set; }
    public DateOnly Date { get; set; }
    public TimeSlotStatus Status { get; set; } = TimeSlotStatus.Available;
    public Guid? BookingId { get; set; }

    // Navigation
    public WashBay WashBay { get; set; } = null!;
    public TimeSlot TimeSlot { get; set; } = null!;
    public Booking? Booking { get; set; }
}