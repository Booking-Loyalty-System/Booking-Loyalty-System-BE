using Domain.Enums;

namespace Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid WashPackageId { get; set; }
    public Guid? TimeSlotId { get; set; }
    public Guid BayId { get; set; }
    public Guid BranchId { get; set; }
    public Guid? AssignedStaffId { get; set; }
    public Guid? PromotionId { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly StartTime { get; set; }

    /// <summary>Final price charged after any promotion discount.</summary>
    public decimal TotalPrice { get; set; }

    /// <summary>Amount discounted by an applied promotion (0 when none).</summary>
    public decimal DiscountAmount { get; set; }
    public BookingStatus Status { get; set; }
    public string? QrData { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Customer Customer { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public WashPackage WashPackage { get; set; } = null!;
    public TimeSlot? TimeSlot { get; set; }
    public Branch Branch { get; set; } = null!;
    public WashBay WashBay { get; set; } = null!;
    public Promotion? Promotion { get; set; }
    public ICollection<BookingAddOn> BookingAddOns { get; set; } = new List<BookingAddOn>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
