namespace Domain.Entities;

public class AddOn
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    /// <summary>Extra price added to the booking when this add-on is selected.</summary>
    public decimal Price { get; set; }

    /// <summary>Extra minutes this add-on adds to the wash duration (0 when none).</summary>
    public int DurationMinutes { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<BookingAddOn> BookingAddOns { get; set; } = new List<BookingAddOn>();
}
