namespace Domain.Entities;

public class Reward
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PointsRequired { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool Status { get; set; }

    public ICollection<Booking> Bookings = new List<Booking>();
}