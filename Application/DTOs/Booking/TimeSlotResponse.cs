namespace Application.DTOs.Booking;

public class TimeSlotResponse
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int CurrentBookings { get; set; }
    public int MaxCapacity { get; set; }
    public bool IsAvailable { get; set; }
}
