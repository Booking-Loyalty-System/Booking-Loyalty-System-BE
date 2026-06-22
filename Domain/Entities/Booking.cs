using Domain.Enums;

namespace Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid WashPackageId { get; set; }
    public Guid BranchTimeSlotId { get; set; } 
    
    public Guid? BayId { get; set; }
    public Guid? StaffId { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public Guid? RewardId { get; set; }
    public Guid? PromotionId { get; set; }
    
    // XÓA: BookingDate, StartTime, EndTime (Vì lấy từ WashBayTimeSlot -> TimeSlot ra là có hết)
    public string? CustomerNote { get; set; }
    public decimal TotalPrice { get; set; }

    /// <summary>Amount discounted by an applied promotion (0 when none).</summary>
    public decimal DiscountAmount { get; set; }
    public BookingStatus Status { get; set; }
    public string? QrData { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public Customer Customer { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public WashPackage WashPackage { get; set; } = null!;
    
    // ĐỔI Ở ĐÂY: Trỏ đến bảng trung gian
    public BranchTimeSlot BranchTimeSlot { get; set; } = null!; 
    public WashBay? WashBay { get; set; }
    public Staff? Staff { get; set; }
    public Reward? Reward { get; set; }
    public Promotion? Promotion { get; set; }
    public ICollection<BookingAddOn> BookingAddOns { get; set; } = new List<BookingAddOn>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
