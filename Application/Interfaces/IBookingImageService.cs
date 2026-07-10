using Application.DTOs.Booking;

namespace Application.Interfaces;

public interface IBookingImageService
{
    Task<BookingImageResponse> AddAsync(Guid userId, Guid bookingId, AddBookingImageRequest request);
    Task<List<BookingImageResponse>> GetByBookingAsync(Guid bookingId);
    Task DeleteAsync(Guid bookingId, Guid imageId);
}
