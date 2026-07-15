using Application.DTOs.Booking;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BookingImageService : IBookingImageService
{
    private readonly IApplicationDbContext _context;

    public BookingImageService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookingImageResponse> AddAsync(Guid userId, Guid bookingId, AddBookingImageRequest request)
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);

        var type = Enum.Parse<BookingImageType>(request.Type);

        // Chỉ cho gắn ảnh đúng giai đoạn: BeforeWash khi xe đang được xử lý,
        // AfterWash khi đã rửa xong. Tránh staff gắn nhầm mốc.
        var allowed = type switch
        {
            BookingImageType.BeforeWash =>
                booking.Status is BookingStatus.Confirmed or BookingStatus.CheckedIn or BookingStatus.Queued or BookingStatus.InProgress,
            BookingImageType.AfterWash =>
                booking.Status is BookingStatus.InProgress or BookingStatus.Completed or BookingStatus.CheckedOut,
            _ => false
        };
        if (!allowed)
            throw new AppException(
                $"Cannot add a {type} image while the booking is {booking.Status}.", 400);

        // Nhân viên đang gắn ảnh (nếu tài khoản là staff); không bắt buộc.
        var staffId = await _context.Staffs
            .Where(s => s.UserId == userId)
            .Select(s => (Guid?)s.Id)
            .FirstOrDefaultAsync();

        var image = new BookingImage
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            ImageUrl = request.ImageUrl,
            Type = type,
            Note = request.Note,
            UploadedByStaffId = staffId,
            CreatedAt = DateTime.UtcNow
        };

        _context.BookingImages.Add(image);
        await _context.SaveChangesAsync();

        return MapToResponse(image);
    }

    public async Task<List<BookingImageResponse>> GetByBookingAsync(Guid bookingId)
    {
        return await _context.BookingImages
            .Where(i => i.BookingId == bookingId)
            .OrderBy(i => i.CreatedAt)
            .Select(i => new BookingImageResponse
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                Type = i.Type.ToString(),
                Note = i.Note,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid bookingId, Guid imageId)
    {
        var image = await _context.BookingImages
            .FirstOrDefaultAsync(i => i.Id == imageId && i.BookingId == bookingId)
            ?? throw new AppException("Booking image not found.", 404);

        _context.BookingImages.Remove(image);
        await _context.SaveChangesAsync();
    }

    private static BookingImageResponse MapToResponse(BookingImage image) => new()
    {
        Id = image.Id,
        ImageUrl = image.ImageUrl,
        Type = image.Type.ToString(),
        Note = image.Note,
        CreatedAt = image.CreatedAt
    };
}
