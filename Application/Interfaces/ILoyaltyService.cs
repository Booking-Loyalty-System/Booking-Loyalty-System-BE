using Application.DTOs.Loyalty;

namespace Application.Interfaces;

public interface ILoyaltyService
{
    /// <summary>
    /// Awards earn-points for a completed booking and updates the customer's running totals.
    /// Idempotent: a second call for the same booking is a no-op.
    /// </summary>
    Task AwardPointsForBookingAsync(Guid bookingId, CancellationToken cancellationToken = default);

    Task<LoyaltyBalanceResponse> GetBalanceAsync(Guid userId);

    Task<List<LoyaltyTransactionResponse>> GetHistoryAsync(Guid userId);
}
