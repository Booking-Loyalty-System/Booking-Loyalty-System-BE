namespace Application.DTOs.Loyalty;

public class LoyaltyTransactionResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public int Points { get; set; }
    public int BalanceAfter { get; set; }
    public Guid? BookingId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
