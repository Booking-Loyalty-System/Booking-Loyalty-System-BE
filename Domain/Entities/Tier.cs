using Domain.Enums;

namespace Domain.Entities;

public class Tier
{
    public Guid Id { get; set; }
    public string TierName { get; set; } = string.Empty;
    public decimal PointRate { get; set; }
    public int BookingWindow { get; set; }
    public PriorityLevel Level { get; set; }

    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}