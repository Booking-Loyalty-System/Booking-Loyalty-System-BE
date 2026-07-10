namespace Application.DTOs.Booking;

public class BookingImageResponse
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
}
