using Domain.Enums;

namespace Domain.Entities;

public class WashBay
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public WashBayStatus Status { get; set; } = WashBayStatus.Available;
    public string SupportedTypes { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
}
