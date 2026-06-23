namespace Domain.Entities;

/// <summary>
/// A customer's loyalty point balance, keyed by User (per ERD).
/// AvailablePoints = currently spendable; TotalPoints = lifetime cumulative earned.
/// </summary>
public class Point
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    /// <summary>Currently spendable balance.</summary>
    public int AvailablePoints { get; set; }

    /// <summary>Lifetime cumulative points earned (never decreases on spend).</summary>
    public int TotalPoints { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<PointHistory> Histories { get; set; } = new List<PointHistory>();
}
