namespace Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<ServiceFeature> Features { get; set; } = new List<ServiceFeature>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
