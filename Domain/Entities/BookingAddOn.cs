namespace Domain.Entities;

/// <summary>
/// Join row linking a Booking to an AddOn. Price and DurationMinutes are snapshotted
/// at booking time so later edits to the AddOn catalog never change historical bookings.
/// </summary>
public class BookingAddOn
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid AddOnId { get; set; }

    /// <summary>Price of the add-on at the time of booking (snapshot).</summary>
    public decimal Price { get; set; }

    /// <summary>Duration of the add-on at the time of booking (snapshot).</summary>
    public int DurationMinutes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Booking Booking { get; set; } = null!;
    public AddOn AddOn { get; set; } = null!;
}
