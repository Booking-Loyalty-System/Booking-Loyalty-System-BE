using Application.DTOs.Booking;

namespace Application.Interfaces;

/// <summary>
/// Staff-side operational workflow over a booking's lifecycle:
/// check-in → queue → start → finish → checkout, plus cancel / no-show.
/// Every mutating method validates the status transition and returns the
/// updated booking so the frontend can refresh without re-fetching the list.
/// </summary>
public interface IStaffBookingService
{
    Task<List<BookingResponseData>> GetByDateAsync(Guid userId, DateOnly date);
    Task<BookingResponseData> ConfirmAsync(Guid bookingId);
    Task<BookingResponseData> CheckInAsync(Guid bookingId, Guid staffId);
    Task<BookingResponseData> QueueAsync(Guid bookingId, Guid washBayId);
    Task<BookingResponseData> StartServiceAsync(Guid bookingId, Guid? bayId);
    Task<BookingResponseData> CheckOutAsync(Guid bookingId);
    Task<BookingResponseData> CompleteServiceAsync(Guid bookingId);
    Task<BookingResponseData> CancelAsync(Guid bookingId, string? reason);
    Task<BookingResponseData> NoShowAsync(Guid bookingId);
    Task<BookingResponseData> GetBookingByQrPayloadAsync(string qrPayload);
}
