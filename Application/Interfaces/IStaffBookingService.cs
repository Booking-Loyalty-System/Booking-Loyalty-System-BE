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
    Task<BookingResponseData> UpdateStatusAsync(Guid bookingId, UpdateBookingStatusRequest request);
}
