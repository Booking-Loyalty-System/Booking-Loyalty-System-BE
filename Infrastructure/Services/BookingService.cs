using Application.Common;
using Application.DTOs.Booking;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QRCoder;

namespace Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly ILoyaltyService _loyaltyService;
    private readonly IPromotionService _promotionService;
    private readonly BookingOptions _options;
    private readonly TimeZoneInfo _shopTimeZone;
    private readonly IHubContext<BookingHub> _hubContext;
    private readonly INotificationService _notificationService;

    public BookingService(IApplicationDbContext context, ILoyaltyService loyaltyService, IPromotionService promotionService, BookingOptions options, TimeZoneInfo shopTimeZone, IHubContext<BookingHub> hubContext, INotificationService notificationService)
    {
        _context = context;
        _loyaltyService = loyaltyService;
        _promotionService = promotionService;
        _options = options;
        _shopTimeZone = shopTimeZone;
        _hubContext = hubContext;
        _notificationService = notificationService;
    }

    public async Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request)
{
    // 1. Kiểm tra các thực thể cơ bản
    var customer = await _context.Customers
        .Include(c => c.Tier)
        .FirstOrDefaultAsync(c => c.UserId == userId)
        ?? throw new AppException("Customer profile not found.", 404);

    var vehicle = await _context.Vehicles
        .FirstOrDefaultAsync(v => v.Id == request.VehicleId && v.CustomerId == customer.Id && !v.IsDeleted)
        ?? throw new AppException("Vehicle not found or does not belong to you.", 404);

    var washPackage = await _context.WashPackages
        .FirstOrDefaultAsync(wp => wp.Id == request.WashPackageId && wp.IsActive)
        ?? throw new AppException("Wash package not found or inactive.", 404);

    var branch = await _context.Branches
        .FirstOrDefaultAsync(b => b.Id == request.BranchId)
        ?? throw new AppException("Branch not found.", 404);

    if (branch.Status != BranchStatus.Active)
        throw new AppException("The selected branch is not currently open for booking.", 400);

    // 2. Kiểm tra thời gian thực tại địa phương của Shop
    var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _shopTimeZone);
    var today = DateOnly.FromDateTime(nowLocal);
    var nowTime = TimeOnly.FromDateTime(nowLocal);

    if (request.BookingDate < today)
        throw new AppException("Booking date cannot be in the past.", 400);

    if (request.BookingDate == today && request.StartTime <= nowTime)
        throw new AppException("Start time cannot be in the past.", 400);

    var maxDays = customer.Tier.BookingWindow;
    var maxDate = today.AddDays(maxDays);
    if (request.BookingDate > maxDate)
        throw new AppException($"Your {customer.Tier.TierName} tier allows booking up to {maxDays} days in advance.", 400);

    if (request.StartTime < _options.OpenTime || request.StartTime >= _options.CloseTime)
        throw new AppException($"Booking time must be within business hours ({_options.OpenTime:HH\\:mm}–{_options.CloseTime:HH\\:mm}).", 400);

    // 3. Tìm cấu hình Khung Giờ gốc và bảng trung gian BranchTimeSlot
    var timeSlot = await _context.TimeSlots
        .FirstOrDefaultAsync(t => t.StartTime == request.StartTime)
        ?? throw new AppException("Invalid start time. Time slot not found.", 400);

    var branchTimeSlot = await _context.BranchTimeSlots
        .FirstOrDefaultAsync(bts => bts.BranchId == branch.Id && bts.TimeSlotId == timeSlot.Id)
        ?? throw new AppException("This time slot is not available for the selected branch.", 400);

    if (!branchTimeSlot.IsActive)
        throw new AppException("This time slot is currently locked or inactive at this branch.", 400);

    // 4. Chặn trùng lịch của chiếc xe này trong cùng khung giờ
    var vehicleConflict = await _context.Bookings.AnyAsync(b =>
        b.VehicleId == vehicle.Id &&
        b.BookingDate == request.BookingDate &&
        b.BranchTimeSlot.TimeSlotId == timeSlot.Id &&
        (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.InProgress));

    if (vehicleConflict)
        throw new AppException("This vehicle already has a booking that overlaps the selected time.", 409);

    // 5. Mở TRANSACTION chống Race Condition khi đặt chỗ cùng giây
    await using var transaction = await _context.BeginTransactionAsync();

    // Đã sửa: Tính toán những xe đang chiếm chỗ (Bỏ Cancelled và NoShow)
    var currentBookingsCount = await _context.Bookings
        .CountAsync(b => b.BranchTimeSlotId == branchTimeSlot.Id 
                      && b.BookingDate == request.BookingDate 
                      && b.Status != BookingStatus.Cancelled
                      && b.Status != BookingStatus.NoShow);

    if (currentBookingsCount >= branchTimeSlot.MaxCapacity)
        throw new AppException("This time slot is fully booked. Please select another time or branch.", 409);

    // 6. Xử lý mã Code, QR và Promotion
    var bookingCode = await GenerateUniqueBookingCodeAsync();
    var qrDataBase64 = GenerateQrCodeBase64(bookingCode);
    
    var totalPrice = washPackage.Price;
    var discountAmount = 0m;
    Guid? promotionId = null;
    
    if (!string.IsNullOrWhiteSpace(request.PromotionCode))
    {
        var (pid, discount) = await _promotionService.ApplyAsync(request.PromotionCode, washPackage.Price);
        promotionId = pid;
        discountAmount = discount;
        totalPrice = washPackage.Price - discount;
    }

    // 7. Khởi tạo và lưu Booking mới vào DB (ĐÃ ĐỒI THEO DB MỚI)
    var booking = new Booking
    {
        Id = Guid.NewGuid(),
        BookingCode = bookingCode,
        CustomerId = customer.Id,
        VehicleId = vehicle.Id,
        WashPackageId = washPackage.Id,
        BranchTimeSlotId = branchTimeSlot.Id, 
        BookingDate = request.BookingDate,   
        BayId = null,                
        QrData = qrDataBase64,
        PromotionId = promotionId,
        TotalPrice = totalPrice,
        StartTime = branchTimeSlot.TimeSlot.StartTime,
        DiscountAmount = discountAmount,
        Status = BookingStatus.Pending,
        CreatedAt = DateTime.UtcNow
    };

    _context.Bookings.Add(booking);
    await _context.SaveChangesAsync();
    
    // Commit transaction an toàn
    await transaction.CommitAsync();

    try 
    {
        string message = $"Khách hàng mới đã đặt lịch: {booking.BookingCode} - Gói: {washPackage.Name} lúc {booking.StartTime}";
        
        // Gọi service gửi thông báo đến các nhân viên thuộc chi nhánh này
        await _notificationService.SendNotificationToStaffAsync(
            branchId: branch.Id, 
            title: "Lịch hẹn mới", 
            message: message,
            relatedId: booking.Id,
            type: "NewBooking"
        );
    }
    catch (Exception ex)
    {
        // Log lỗi nếu gửi thông báo thất bại nhưng không làm gián đoạn luồng đặt lịch
        Console.WriteLine($"Gửi thông báo thất bại: {ex.Message}");
    }
    
    // 8. Trả kết quả map dữ liệu và bắn SignalR Realtime báo cho chi nhánh biết
    var response = MapToResponse(booking, washPackage, vehicle, timeSlot, branch, null);
    
    await _hubContext.Clients.Group(branchTimeSlot.BranchId.ToString())
        .SendAsync("ReceiveBookingCreated", response);
        
    return response;
}

    public async Task<BookingResponse> GetBookingByIdAsync(Guid userId, Guid bookingId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.TimeSlot)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.Branch)
            .Include(b => b.WashBay) // Lấy thông tin bệ rửa nếu đã được xếp
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.BranchTimeSlot.TimeSlot, booking.BranchTimeSlot.Branch, booking.WashBay);
    }

    public async Task<List<BookingResponse>> GetMyBookingsAsync(Guid userId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var bookings = await _context.Bookings
            .Where(b => b.CustomerId == customer.Id)
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.TimeSlot)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.Branch)
            .Include(b => b.WashBay)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return bookings.Select(b => MapToResponse(b, b.WashPackage, b.Vehicle, b.BranchTimeSlot.TimeSlot, b.BranchTimeSlot.Branch, b.WashBay)).ToList();
    }

    public async Task<BookingResponse> CancelBookingAsync(Guid userId, Guid bookingId, string? reason)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.TimeSlot)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.Branch)
            .Include(b => b.WashBay)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Pending or Confirmed bookings can be cancelled.", 400);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;

        // Chỉ cần đổi status, Capacity sẽ tự động nới lỏng ra nhờ câu lệnh CountAsync không đếm Cancelled.

        await _context.SaveChangesAsync();

        var response = MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.BranchTimeSlot.TimeSlot, booking.BranchTimeSlot.Branch, booking.WashBay);
        
        await _hubContext.Clients.Group(booking.BranchTimeSlot.BranchId.ToString())
            .SendAsync("ReceiveBookingCancelled", new { BookingId = booking.Id, Reason = reason });
            
        return response;
    }

    public async Task<BookingResponse> UpdateBookingAsync(Guid userId, Guid bookingId, UpdateBookingRequest request)
    {
        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.TimeSlot)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.Branch)
            .Include(b => b.WashBay)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Pending or Confirmed bookings can be modified.", 400);

        var targetBranchId = request.BranchId ?? booking.BranchTimeSlot.BranchId;
        var targetWashPackageId = request.WashPackageId ?? booking.WashPackageId;
        var targetBookingDate = request.BookingDate ?? booking.BookingDate;
        var targetStartTime = request.StartTime ?? booking.BranchTimeSlot.TimeSlot.StartTime;

        bool isBranchChanged = targetBranchId != booking.BranchTimeSlot.BranchId;
        bool isPackageChanged = targetWashPackageId != booking.WashPackageId;
        bool isTimeChanged = targetBookingDate != booking.BookingDate || targetStartTime != booking.BranchTimeSlot.TimeSlot.StartTime;

        if (!isBranchChanged && !isPackageChanged && !isTimeChanged)
        {
            throw new AppException("No changes detected in your update request.", 400);
        }

        if (isPackageChanged)
        {
            var newPackage = await _context.WashPackages
                .FirstOrDefaultAsync(wp => wp.Id == targetWashPackageId && wp.IsActive)
                ?? throw new AppException("New wash package not found or inactive.", 404);
            
            booking.WashPackageId = newPackage.Id;
            booking.TotalPrice = newPackage.Price; 
        }

        if (isBranchChanged || isTimeChanged)
        {
            var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == targetBranchId)
                ?? throw new AppException("Target branch not found.", 404);

            if (branch.Status != BranchStatus.Active)
                throw new AppException("The selected branch is not active.", 400);

            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _shopTimeZone);
            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            if (targetBookingDate < today)
                throw new AppException("New booking date cannot be in the past.", 400);

            if (targetBookingDate == today && targetStartTime <= nowTime)
                throw new AppException("New start time cannot be in the past.", 400);

            var maxDays = customer.Tier.BookingWindow;
            if (targetBookingDate > today.AddDays(maxDays))
                throw new AppException($"Your tier allows booking up to {maxDays} days in advance.", 400);

            if (targetStartTime < _options.OpenTime || targetStartTime >= _options.CloseTime)
                throw new AppException($"Booking time must be within business hours ({_options.OpenTime:HH\\:mm}–{_options.CloseTime:HH\\:mm}).", 400);

            var newTimeSlot = await _context.TimeSlots
                .FirstOrDefaultAsync(t => t.StartTime == targetStartTime)
                ?? throw new AppException("Invalid start time. Time slot not found.", 400);

            var newBranchTimeSlot = await _context.BranchTimeSlots
                .FirstOrDefaultAsync(bts => bts.BranchId == targetBranchId && bts.TimeSlotId == newTimeSlot.Id)
                ?? throw new AppException("This time slot is not available for the selected branch.", 400);

            if (!newBranchTimeSlot.IsActive)
                throw new AppException("This time slot is currently locked or inactive at this branch.", 400);

            var vehicleConflict = await _context.Bookings.AnyAsync(b =>
                b.Id != booking.Id &&
                b.VehicleId == booking.VehicleId &&
                b.BookingDate == targetBookingDate &&
                b.BranchTimeSlotId == newBranchTimeSlot.Id &&
                (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.InProgress));

            if (vehicleConflict)
                throw new AppException("This vehicle already has another booking at the selected time.", 409);

            await using var transaction = await _context.BeginTransactionAsync();

            var currentBookingsCount = await _context.Bookings
                .CountAsync(b => b.BranchTimeSlotId == newBranchTimeSlot.Id 
                              && b.BookingDate == targetBookingDate 
                              && b.Status != BookingStatus.Cancelled);

            if (currentBookingsCount >= newBranchTimeSlot.MaxCapacity)
                throw new AppException("This new time slot is fully booked. Please select another time or branch.", 409);

            // Đổi lịch: Cập nhật thông tin và NHẢ BỆ RỬA CŨ (nếu đã được gán)
            booking.BranchTimeSlotId = newBranchTimeSlot.Id;
            booking.BookingDate = targetBookingDate;
            booking.BayId = null; // Khách đổi giờ nên phải chờ nhân viên xếp lại bệ khác
            booking.UpdatedAt = DateTime.UtcNow;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var updatedBooking = await _context.Bookings
                .Include(b => b.WashPackage)
                .Include(b => b.Vehicle)
                .Include(b => b.BranchTimeSlot)
                    .ThenInclude(bts => bts.TimeSlot)
                .Include(b => b.BranchTimeSlot)
                    .ThenInclude(bts => bts.Branch)
                .Include(b => b.WashBay)
                .FirstAsync(b => b.Id == booking.Id);

            return MapToResponse(updatedBooking, updatedBooking.WashPackage, updatedBooking.Vehicle, updatedBooking.BranchTimeSlot.TimeSlot, updatedBooking.BranchTimeSlot.Branch, updatedBooking.WashBay);
        }

        booking.UpdatedAt = DateTime.UtcNow;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.BranchTimeSlot.TimeSlot, booking.BranchTimeSlot.Branch, booking.WashBay);
    }
    
    public async Task<BookingResponse> CompleteBookingAsync(Guid bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.TimeSlot)
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.Branch)
            .Include(b => b.WashBay)
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status is BookingStatus.Pending or BookingStatus.Cancelled)
            throw new AppException("Only confirmed or in-progress bookings can be completed.", 400);

        if (booking.Status != BookingStatus.Completed)
        {
            booking.Status = BookingStatus.Completed;
            booking.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        await _loyaltyService.AwardPointsForBookingAsync(bookingId);

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.BranchTimeSlot.TimeSlot, booking.BranchTimeSlot.Branch, booking.WashBay);
    }

    private async Task<string> GenerateUniqueBookingCodeAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        string code;

        do
        {
            code = new string(Enumerable.Range(0, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        } while (await _context.Bookings.AnyAsync(b => b.BookingCode == code));

        return code;
    }

    private string GenerateQrCodeBase64(string text)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        using var bitmapByteQrCode = new BitmapByteQRCode(qrCodeData);
        
        byte[] qrCodeAsBmpByteArr = bitmapByteQrCode.GetGraphic(2); 

        string base64String = Convert.ToBase64String(qrCodeAsBmpByteArr);

        return $"data:image/bmp;base64,{base64String}";
    }
    
    private static BookingResponse MapToResponse(
        Booking booking, WashPackage washPackage, Vehicle vehicle, TimeSlot timeSlot, Branch branch, WashBay? washBay)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            WashPackageName = washPackage.Name,
            DurationMinutes = washPackage.DurationMinutes,
            BookingDate = booking.BookingDate, // Lấy thẳng ngày từ Booking
            StartTime = timeSlot.StartTime, 
            EndTime = timeSlot.StartTime.Add(TimeSpan.FromMinutes(washPackage.DurationMinutes)), 
            WashBayName = washBay?.Name ?? "Not Assigned Yet", // Bệ rửa nếu null sẽ báo chưa xếp bệ
            VehiclePlate = vehicle.LicensePlate,
            VehicleName = vehicle.VehicleName,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            QrData = booking.QrData,
            CreatedAt = booking.CreatedAt,
            BranchId = branch.Id,
            BranchName = branch.BranchName
        };
    }
}