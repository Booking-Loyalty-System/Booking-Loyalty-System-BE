using Application.DTOs.Booking;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;

    public BookingService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request)
    {
        // 1. Find customer from UserId
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        // 2. Validate vehicle belongs to customer & not deleted
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == request.VehicleId && v.CustomerId == customer.Id && !v.IsDeleted)
            ?? throw new AppException("Vehicle not found or does not belong to you.", 404);

        // 3. Validate service exists & active
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == request.ServiceId && s.IsActive)
            ?? throw new AppException("Service not found or inactive.", 404);

        // 4. Validate store exists & active
        var store = await _context.Stores
            .FirstOrDefaultAsync(s => s.Id == request.StoreId && s.IsActive)
            ?? throw new AppException("Store not found or inactive.", 404);

        // 5. Validate booking date not in the past
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (request.BookingDate < today)
            throw new AppException("Booking date cannot be in the past.", 400);

        // 6. Tier-based booking window check
        var maxDays = GetMaxBookingDays(customer.Tier);
        var maxDate = today.AddDays(maxDays);
        if (request.BookingDate > maxDate)
            throw new AppException($"Your {customer.Tier} tier allows booking up to {maxDays} days in advance.", 400);

        // 7. Validate start time within store hours
        if (request.StartTime < store.OpenTime || request.StartTime >= store.CloseTime)
            throw new AppException($"Start time must be between {store.OpenTime} and {store.CloseTime}.", 400);

        // Ensure start time is on the hour
        if (request.StartTime.Minute != 0 || request.StartTime.Second != 0)
            throw new AppException("Start time must be on the hour (e.g., 08:00, 09:00).", 400);

        // Calculate end time
        var endTime = request.StartTime.AddMinutes(service.DurationMinutes);
        if (endTime > store.CloseTime)
            throw new AppException("Service would end after store closing time.", 400);

        // 8. Slot capacity check - check ALL covered hourly slots
        var slotsNeeded = (int)Math.Ceiling(service.DurationMinutes / 60.0);
        var activeStatuses = new[] { BookingStatus.Pending, BookingStatus.Confirmed, BookingStatus.InProgress };

        for (int i = 0; i < slotsNeeded; i++)
        {
            var slotTime = request.StartTime.AddMinutes(i * 60);

            // Count bookings that overlap with this hourly slot
            var bookingsInSlot = await _context.Bookings
                .CountAsync(b => b.StoreId == request.StoreId
                    && b.BookingDate == request.BookingDate
                    && activeStatuses.Contains(b.Status)
                    && b.StartTime <= slotTime
                    && b.EndTime > slotTime);

            if (bookingsInSlot >= store.SlotCapacity)
                throw new AppException($"Time slot {slotTime:HH:mm} is fully booked.", 409);
        }

        // 9. Generate booking code
        var bookingCode = await GenerateUniqueBookingCodeAsync();

        // 10. Create booking
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            BookingCode = bookingCode,
            CustomerId = customer.Id,
            VehicleId = vehicle.Id,
            ServiceId = service.Id,
            StoreId = store.Id,
            BookingDate = request.BookingDate,
            StartTime = request.StartTime,
            EndTime = endTime,
            TotalPrice = service.BasePrice,
            Status = BookingStatus.Confirmed,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // 11. Return response
        return MapToResponse(booking, service, vehicle, store);
    }

    public async Task<BookingResponse> GetBookingByIdAsync(Guid userId, Guid bookingId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.Service)
            .Include(b => b.Vehicle)
            .Include(b => b.Store)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        return MapToResponse(booking, booking.Service, booking.Vehicle, booking.Store);
    }

    public async Task<List<BookingResponse>> GetMyBookingsAsync(Guid userId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var bookings = await _context.Bookings
            .Where(b => b.CustomerId == customer.Id)
            .Include(b => b.Service)
            .Include(b => b.Vehicle)
            .Include(b => b.Store)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return bookings.Select(b => MapToResponse(b, b.Service, b.Vehicle, b.Store)).ToList();
    }

    public async Task<BookingResponse> CancelBookingAsync(Guid userId, Guid bookingId, string? reason)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.Service)
            .Include(b => b.Vehicle)
            .Include(b => b.Store)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Pending or Confirmed bookings can be cancelled.", 400);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(booking, booking.Service, booking.Vehicle, booking.Store);
    }

    public async Task<AvailableSlotsResponse> GetAvailableSlotsAsync(Guid storeId, DateOnly date)
    {
        var store = await _context.Stores
            .FirstOrDefaultAsync(s => s.Id == storeId && s.IsActive)
            ?? throw new AppException("Store not found or inactive.", 404);

        var slots = await GenerateTimeSlotsAsync(store, date);

        return new AvailableSlotsResponse
        {
            Date = date,
            StoreId = store.Id,
            StoreName = store.Name,
            Slots = slots
        };
    }

    public async Task<List<BookingCalendarResponse>> GetCalendarAsync(Guid storeId, DateOnly startDate, DateOnly endDate)
    {
        var store = await _context.Stores
            .FirstOrDefaultAsync(s => s.Id == storeId && s.IsActive)
            ?? throw new AppException("Store not found or inactive.", 404);

        if (endDate < startDate)
            throw new AppException("End date must be after start date.", 400);

        var maxRange = startDate.AddDays(14);
        if (endDate > maxRange)
            endDate = maxRange;

        var result = new List<BookingCalendarResponse>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var slots = await GenerateTimeSlotsAsync(store, date);
            result.Add(new BookingCalendarResponse
            {
                Date = date,
                DayOfWeek = date.DayOfWeek.ToString(),
                Slots = slots
            });
        }

        return result;
    }

    private async Task<List<TimeSlotResponse>> GenerateTimeSlotsAsync(Store store, DateOnly date)
    {
        var activeStatuses = new[] { BookingStatus.Pending, BookingStatus.Confirmed, BookingStatus.InProgress };

        // Get all active bookings for this store and date
        var bookings = await _context.Bookings
            .Where(b => b.StoreId == store.Id
                && b.BookingDate == date
                && activeStatuses.Contains(b.Status))
            .ToListAsync();

        var slots = new List<TimeSlotResponse>();
        var currentTime = store.OpenTime;

        while (currentTime < store.CloseTime)
        {
            var slotEnd = currentTime.AddMinutes(60);
            if (slotEnd > store.CloseTime)
                slotEnd = store.CloseTime;

            // Count bookings that overlap with this slot
            var count = bookings.Count(b => b.StartTime <= currentTime && b.EndTime > currentTime);

            slots.Add(new TimeSlotResponse
            {
                StartTime = currentTime,
                EndTime = slotEnd,
                CurrentBookings = count,
                MaxCapacity = store.SlotCapacity,
                IsAvailable = count < store.SlotCapacity
            });

            currentTime = currentTime.AddMinutes(60);
        }

        return slots;
    }

    private static int GetMaxBookingDays(CustomerTier tier) => tier switch
    {
        CustomerTier.Member => 7,
        CustomerTier.Silver => 10,
        CustomerTier.Gold => 12,
        CustomerTier.Platinum => 14,
        _ => 7
    };

    private async Task<string> GenerateUniqueBookingCodeAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        string code;

        do
        {
            code = "BKN" + new string(Enumerable.Range(0, 8).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        } while (await _context.Bookings.AnyAsync(b => b.BookingCode == code));

        return code;
    }

    private static BookingResponse MapToResponse(Booking booking, Service service, Vehicle vehicle, Store store)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            ServiceName = service.Name,
            DurationMinutes = service.DurationMinutes,
            BookingDate = booking.BookingDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            VehiclePlate = vehicle.LicensePlate,
            VehicleName = vehicle.VehicleName,
            StoreName = store.Name,
            StoreAddress = store.Address,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            CreatedAt = booking.CreatedAt
        };
    }
}
