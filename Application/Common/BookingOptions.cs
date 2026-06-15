namespace Application.Common;

/// <summary>
/// Shop-wide booking rules bound from the "Booking" configuration section.
/// Drives operating-hours validation and timezone-aware "now" in BookingService.
/// </summary>
public class BookingOptions
{
    /// <summary>Windows ("SE Asia Standard Time") or IANA ("Asia/Ho_Chi_Minh") id.</summary>
    public string TimeZoneId { get; set; } = "SE Asia Standard Time";

    /// <summary>Earliest start time the shop accepts (inclusive).</summary>
    public TimeOnly OpenTime { get; set; } = new(8, 0);

    /// <summary>Latest end time the shop accepts (inclusive).</summary>
    public TimeOnly CloseTime { get; set; } = new(20, 0);
}
