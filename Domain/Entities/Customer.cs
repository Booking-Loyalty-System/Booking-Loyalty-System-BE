using Domain.Enums;

namespace Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsPhoneNumberVerified { get; set; } = false;
    public DateTime? DateOfBirth { get; set; }
    public Guid TierId { get; set; }
    public int TotalWashes { get; set; }

    /// <summary>
    /// Số lần rửa đã tích trong chu kỳ hiện tại (0..6). Đủ 7 sẽ tự tặng 1 voucher rửa free
    /// rồi reset về 0. Khác với <see cref="TotalWashes"/> (lũy kế trọn đời, dùng cho thống kê/hạng).
    /// FE đọc field này để hiển thị tiến độ "x/7" của thẻ tích điểm rửa xe.
    /// </summary>
    public int CurrentCycleWashes { get; set; }

    public decimal TotalSpent { get; set; }

    /// <summary>
    /// Thời điểm gần nhất khách bị worker HẠ 1 bậc hạng. Dùng để giới hạn: mỗi tháng dương lịch
    /// chỉ hạ tối đa 1 bậc — tránh rớt thẳng nhiều bậc (vd Kim Cương → Đồng) trong cùng một tháng.
    /// Null = chưa từng bị hạ.
    /// </summary>
    public DateTime? LastTierDowngradeAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
    public Tier Tier { get; set; } = null!;
    public ChatMessage? ChatMessage { get; set; }
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
