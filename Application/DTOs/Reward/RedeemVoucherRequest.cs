namespace Application.DTOs.Reward;

public class RedeemVoucherRequest
{
    /// <summary>The reward milestone the customer wants to redeem points for.</summary>
    public Guid RewardId { get; set; }
}
