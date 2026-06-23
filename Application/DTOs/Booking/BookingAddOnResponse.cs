namespace Application.DTOs.Booking;

public class BookingAddOnResponse
{
    public Guid AddOnId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}
