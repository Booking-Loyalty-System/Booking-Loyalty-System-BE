namespace Application.DTOs.Booking;

public class BookingCalendarResponse
{
    public DateOnly Date { get; set; }
    public string DayOfWeek { get; set; } = null!;
    public List<TimeSlotResponse> Slots { get; set; } = new();
}
