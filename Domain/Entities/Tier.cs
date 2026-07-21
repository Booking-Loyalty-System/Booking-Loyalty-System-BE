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
    /// Số booking hoàn tất tối thiểu trong ~30 ngày gần nhất để GIỮ hạng này.
    /// Dưới ngưỡng (kể cả 0 booking) → bị hạ xuống hạng thấp nhất còn duy trì được.
    /// </summary>
    public int MaintenanceBookings { get; set; }

    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    public ICollection<TierPromotion> TierPromotions { get; set; } = new List<TierPromotion>();
}