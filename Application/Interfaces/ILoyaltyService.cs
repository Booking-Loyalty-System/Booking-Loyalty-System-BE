using Application.DTOs.Loyalty;

namespace Application.Interfaces;

public interface ILoyaltyService
{
    /// <summary>
    /// Awards earn-points for a completed booking and updates the customer's running totals.
    /// Idempotent: a second call for the same booking is a no-op.
    /// </summary>
    Task AwardPointsForBookingAsync(Guid bookingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deducts the configured no-show penalty (spendable points only, clamped at 0) for a
    /// booking that was marked NoShow. Idempotent: one Penalty ledger row per booking.
    /// </summary>
    Task ApplyNoShowPenaltyAsync(Guid bookingId, CancellationToken cancellationToken = default);

    Task<LoyaltyBalanceResponse> GetBalanceAsync(Guid userId);

    Task<List<LoyaltyTransactionResponse>> GetHistoryAsync(Guid userId);
}
