using Domain.Entities;
using Domain.Enums;

public class LoyaltyTransaction
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public LoyaltyTransactionType Type { get; set; } // Earn, Redeem, Expire, RewardAdjustment...

    // Dành cho biến động điểm (Transaction)
    public int Points { get; set; } 
    public int BalanceAfter { get; set; }
    public Guid? BookingId { get; set; }

    // Dành cho thông tin bổ trợ (History/Reward)
    public Guid? RewardId { get; set; } // Null nếu không liên quan phần thưởng
    public string Description { get; set; } = string.Empty;

    // Thời gian
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; } 

    // Navigation
    public Customer Customer { get; set; } = null!;
    public Booking? Booking { get; set; }
    public Reward? Reward { get; set; }
}