namespace Application.DTOs.Booking;

public class AvailableSlotsResponse
{
    public DateOnly Date { get; set; }
    public Guid StoreId { get; set; }
    public string StoreName { get; set; } = null!;
    public List<TimeSlotResponse> Slots { get; set; } = new();
}
