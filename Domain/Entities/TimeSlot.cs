using Domain.Enums;

namespace Domain.Entities;

public class TimeSlot
{
    public Guid Id { get; set; }
    public TimeOnly StartTime { get; set; }

    public ICollection<WashBayTimeSlot> WashBayTimeSlots { get; set; } = new List<WashBayTimeSlot>();
}
