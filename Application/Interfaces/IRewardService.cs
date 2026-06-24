using Application.DTOs.Reward;

namespace Application.Interfaces;

public interface IRewardService
{
    // Catalog management
    Task<List<RewardResponse>> GetAllAsync(bool activeOnly);
    Task<RewardResponse?> GetByIdAsync(Guid id);
    Task<RewardResponse> CreateAsync(CreateRewardRequest request);
    Task<RewardResponse> UpdateAsync(Guid id, UpdateRewardRequest request);
    Task DeleteAsync(Guid id);

    // Redemption
    /// <summary>Spends the customer's points on a reward. Atomic and balance-checked.</summary>
    Task<RedemptionResponse> RedeemAsync(Guid userId, Guid rewardId);

    Task<List<RedemptionResponse>> GetMyRedemptionsAsync(Guid userId);

    /// <summary>Staff marks a pending redemption as handed over to the customer.</summary>
    Task<RedemptionResponse> FulfillAsync(Guid redemptionId);

    // ----- Voucher contract (loyalty FE) -----

    /// <summary>The customer's personal vouchers (redeemed rewards), Active first then by expiry.</summary>
    Task<List<VoucherResponse>> GetMyVouchersAsync(Guid userId);

    /// <summary>Spends points to redeem a reward and returns the resulting voucher.</summary>
    Task<VoucherResponse> RedeemVoucherAsync(Guid userId, Guid rewardId);

    /// <summary>Marks the customer's own voucher as used (consumed).</summary>
    Task UseVoucherAsync(Guid userId, Guid voucherId);
}
