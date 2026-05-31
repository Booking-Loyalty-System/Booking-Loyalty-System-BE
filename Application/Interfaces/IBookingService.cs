using Application.DTOs.Booking;

namespace Application.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request);
    Task<BookingResponse> GetBookingByIdAsync(Guid userId, Guid bookingId);
    Task<List<BookingResponse>> GetMyBookingsAsync(Guid userId);
    Task<BookingResponse> CancelBookingAsync(Guid userId, Guid bookingId, string? reason);
    Task<AvailableSlotsResponse> GetAvailableSlotsAsync(Guid storeId, DateOnly date);
    Task<List<BookingCalendarResponse>> GetCalendarAsync(Guid storeId, DateOnly startDate, DateOnly endDate);
}
