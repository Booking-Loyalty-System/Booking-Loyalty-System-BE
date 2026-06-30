namespace Application.Common;

/// <summary>
/// Loyalty rules bound from the "Loyalty" configuration section.
/// Drives point earning and expiry in LoyaltyService.
/// </summary>
public class LoyaltyOptions
{
    /// <summary>
    /// Points awarded per one currency unit spent. Example: 0.001 = 1 point per 1,000 VND.
    /// Earned points = floor(booking total price * PointsPerCurrencyUnit).
    /// </summary>
    public decimal PointsPerCurrencyUnit { get; set; } = 0.001m;

    /// <summary>Months until earned points expire (brief: 12 months).</summary>
    public int PointLifetimeMonths { get; set; } = 12;

    /// <summary>
    /// Spendable points deducted from a customer when a booking is marked NoShow.
    /// Only AvailablePoints is reduced (never TotalPoints), and the balance is clamped at 0.
    /// </summary>
    public int NoShowPenaltyPoints { get; set; } = 50;
}
