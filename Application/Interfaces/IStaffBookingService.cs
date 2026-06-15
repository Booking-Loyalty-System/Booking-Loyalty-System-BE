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
    Task<List<BookingResponseData>> GetByDateAsync(DateOnly date);
    Task<BookingResponseData> CheckInAsync(Guid bookingId);
    Task<BookingResponseData> QueueAsync(Guid bookingId);
    Task<BookingResponseData> StartAsync(Guid bookingId, Guid staffId);
    Task<BookingResponseData> FinishAsync(Guid bookingId);
    Task<BookingResponseData> CheckoutAsync(Guid bookingId);
    Task<BookingResponseData> CancelAsync(Guid bookingId, string? reason);
    Task<BookingResponseData> NoShowAsync(Guid bookingId);
}
