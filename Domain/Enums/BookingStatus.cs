namespace Domain.Enums;

public enum BookingStatus
{
    Pending,
    Confirmed,
    InProgress,
    Completed,
    Cancelled,
    // Staff operational flow (appended to preserve existing stored values).
    CheckedIn,
    Queued,
    CheckedOut,
    NoShow
}
