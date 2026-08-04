using Domain.Enums;

namespace Domain.Entities;

public class Tier
{
    public Guid Id { get; set; }
    public string TierName { get; set; } = string.Empty;
    public decimal PointRate { get; set; }
    public int BookingWindow { get; set; }
    public PriorityLevel Level { get; set; }
    public int MinPointsRequired { get; set; }
    public int MaintenancePoints { get; set; }

    /// <summary>
    /// KHÔNG còn dùng trong nghiệp vụ. Trước đây là số booking tối thiểu trong ~30 ngày để giữ hạng,
    /// phục vụ cơ chế hạ hạng — cơ chế này đã được bỏ, hạng nay chỉ xét theo <see cref="MinPointsRequired"/>
    /// và không bao giờ giảm. Giữ lại để không phải đổi schema; không đọc, không ghi ở luồng nào.
    /// </summary>
    public int MaintenanceBookings { get; set; }

    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    public ICollection<TierPromotion> TierPromotions { get; set; } = new List<TierPromotion>();
}