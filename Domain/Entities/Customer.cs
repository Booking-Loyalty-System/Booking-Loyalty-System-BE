using Domain.Enums;

namespace Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsPhoneNumberVerified { get; set; } = false;
    public DateTime? DateOfBirth { get; set; }
    public CustomerTier Tier { get; set; } = CustomerTier.Member;
    public int TotalPoints { get; set; }
    public int LifetimePoints { get; set; }
    public int TotalWashes { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
