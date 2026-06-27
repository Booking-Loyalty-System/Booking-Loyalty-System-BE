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

    /// <summary>
    /// Minutes after a booking's start time before an un-started Confirmed/CheckedIn
    /// booking is auto-marked NoShow by the background sweeper.
    /// </summary>
    public int NoShowGraceMinutes { get; set; } = 30;

    /// <summary>
    /// Max number of unconfirmed (Pending) bookings a single customer may hold at once.
    /// In the no-prepayment flow there is no deposit to deter slot hoarding, so this caps
    /// how many slots one customer can hold while waiting for staff to call and confirm.
    /// </summary>
    public int MaxActivePendingBookings { get; set; } = 5;
}
