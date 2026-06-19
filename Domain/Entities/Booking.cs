using Domain.Enums;

namespace Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid WashPackageId { get; set; }
    
    // ĐỔI Ở ĐÂY: Thay vì lưu rời rạc SlotId và BayId, ông chỉ cần trỏ tới ID của bảng trung gian
    public Guid WashBayTimeSlotId { get; set; } 
    
    public Guid BranchId { get; set; }
    public Guid? AssignedStaffId { get; set; }
    public Guid? PromotionId { get; set; }
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
    public WashBayTimeSlot WashBayTimeSlot { get; set; } = null!; 
    
    public Branch Branch { get; set; } = null!;
    public Reward? Reward { get; set; }
    public Promotion? Promotion { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
