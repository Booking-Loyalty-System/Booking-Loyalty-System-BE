using Domain.Enums;

namespace Domain.Entities;

public class WashBay
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public WashBayStatus Status { get; set; } = WashBayStatus.Available;
    public string SupportedTypes { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid BranchId { get; set; }

    public Branch Branch { get; set; } = null!;
    // Navigation
    public ICollection<WashBayTimeSlot> WashBayTimeSlots { get; set; } = new List<WashBayTimeSlot>();
}
