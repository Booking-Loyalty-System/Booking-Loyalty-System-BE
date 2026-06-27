using Application.DTOs.Booking;

namespace Application.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request);
    Task<BookingResponse> GetBookingByIdAsync(Guid userId, Guid bookingId);
    Task<List<BookingResponse>> GetMyBookingsAsync(Guid userId);
    Task<BookingResponse> CancelBookingAsync(Guid userId, Guid bookingId, string? reason);
    Task<BookingResponse> UpdateBookingAsync(Guid userId, Guid bookingId, UpdateBookingRequest request);
    Task<BookingResponse> CompleteBookingAsync(Guid bookingId);
}
