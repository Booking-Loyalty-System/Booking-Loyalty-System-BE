namespace Application.DTOs.Loyalty;

public class LoyaltyBalanceResponse
{
    public int TotalPoints { get; set; }
    public int LifetimePoints { get; set; }
    public int TotalWashes { get; set; }
    public decimal TotalSpent { get; set; }
    public string Tier { get; set; } = null!;
}
