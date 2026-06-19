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
    private readonly BookingOptions _options;
    private readonly TimeZoneInfo _shopTimeZone;
    private readonly IHubContext<BookingHub> _hubContext;
    public BookingService(
        IApplicationDbContext context,
        ILoyaltyService loyaltyService,
        IOptions<BookingOptions> options,
        IHubContext<BookingHub> hubContext) // Thêm vào constructor
    {
        _context = context;
        _loyaltyService = loyaltyService;
        _options = options.Value;
        _shopTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_options.TimeZoneId);
        _hubContext = hubContext;
    }

    public async Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request)
    {
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

        // Lấy thời gian thực tại tiệm
        var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _shopTimeZone);
        var today = DateOnly.FromDateTime(nowLocal);
        var nowTime = TimeOnly.FromDateTime(nowLocal);

        if (request.BookingDate < today)
            throw new AppException("Booking date cannot be in the past.", 400);

        if (request.BookingDate == today && request.StartTime <= nowTime)
            throw new AppException("Start time cannot be in the past.", 400);

        // Kiểm tra giới hạn ngày đặt của Tier
        var maxDays = customer.Tier.BookingWindow;
        var maxDate = today.AddDays(maxDays);
        if (request.BookingDate > maxDate)
            throw new AppException($"Your {customer.Tier.TierName} tier allows booking up to {maxDays} days in advance.", 400);

        // Đảm bảo khung giờ nằm trong giờ làm việc của tiệm
        if (request.StartTime < _options.OpenTime || request.StartTime >= _options.CloseTime)
            throw new AppException($"Booking time must be within business hours ({_options.OpenTime:HH\\:mm}–{_options.CloseTime:HH\\:mm}).", 400);

        // CHẶN TRÙNG LỊCH XE: Kiểm tra xem xe này đã đặt ô lịch nào trùng Ngày + Giờ đó chưa
        var vehicleConflict = await _context.Bookings.AnyAsync(b =>
            b.VehicleId == vehicle.Id &&
            (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.InProgress) &&
            b.WashBayTimeSlot.Date == request.BookingDate &&
            b.WashBayTimeSlot.TimeSlot.StartTime == request.StartTime);

        if (vehicleConflict)
            throw new AppException("This vehicle already has a booking that overlaps the selected time.", 409);

        // Bắt đầu giao dịch bảo vệ chống Race Condition (Hai khách đặt cùng 1 chỗ 1 lúc)
        await using var transaction = await _context.BeginTransactionAsync();

        // TÌM Ô LỊCH TRỐNG THỰC TẾ: Soi trực tiếp bảng trung gian WashBayTimeSlot theo Ngày + Giờ + Chi nhánh
        var availableSlot = await _context.WashBayTimeSlots
            .Include(wbts => wbts.WashBay)
            .Include(wbts => wbts.TimeSlot)
            .FirstOrDefaultAsync(wbts =>
                wbts.WashBay.BranchId == branch.Id &&
                wbts.Date == request.BookingDate &&
                wbts.TimeSlot.StartTime == request.StartTime &&
                wbts.Booking == null && // Ô lịch phải trống
                wbts.WashBay.Status == WashBayStatus.Available);

        if (availableSlot == null)
            throw new AppException("No wash bay is available at this branch for the selected date and time.", 409);

        // Kiểm tra xem Khoang đó có hỗ trợ loại xe này không (Small, Medium, Large)
        var typeName = vehicle.Type.ToString();
        var supported = availableSlot.WashBay.SupportedTypes
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (supported.Length > 0 && !supported.Any(s => string.Equals(s, typeName, StringComparison.OrdinalIgnoreCase)))
        {
            throw new AppException($"The available wash bay for this slot does not support {typeName} vehicles.", 400);
        }

        // Tạo mã Code độc nhất 6 ký tự
        var bookingCode = await GenerateUniqueBookingCodeAsync();

        var qrDataBase64 = GenerateQrCodeBase64(bookingCode);
        
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            BookingCode = bookingCode,
            CustomerId = customer.Id,
            VehicleId = vehicle.Id,
            WashPackageId = washPackage.Id,
            BranchId = branch.Id,
            WashBayTimeSlotId = availableSlot.Id, 
            TotalPrice = washPackage.Price,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            QrData = qrDataBase64,
            BookingDate = request.BookingDate, 
            StartTime = request.StartTime,
        };

        // Cập nhật trạng thái ô lịch của khoang thành Đã đặt
        availableSlot.Status = TimeSlotStatus.Booked;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        var response = MapToResponse(booking, washPackage, vehicle, availableSlot);
        await _hubContext.Clients.Group(booking.BranchId.ToString())
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
            .Include(b => b.WashBayTimeSlot)
                .ThenInclude(wbts => wbts.WashBay)
            .Include(b => b.WashBayTimeSlot)
                .ThenInclude(wbts => wbts.TimeSlot)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.WashBayTimeSlot);
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
            .Include(b => b.Branch)
            .Include(b => b.WashBayTimeSlot)
                .ThenInclude(wbts => wbts.WashBay)
            .Include(b => b.WashBayTimeSlot)
                .ThenInclude(wbts => wbts.TimeSlot)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return bookings.Select(b => MapToResponse(b, b.WashPackage, b.Vehicle, b.WashBayTimeSlot, b.Branch)).ToList();
    }

    public async Task<BookingResponse> CancelBookingAsync(Guid userId, Guid bookingId, string? reason)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.WashBayTimeSlot)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Pending or Confirmed bookings can be cancelled.", 400);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;

        // Giải phóng ô lịch của khoang rửa để người khác đặt gối đầu vào được luôn
        if (booking.WashBayTimeSlot != null)
        {
            booking.WashBayTimeSlot.Status = TimeSlotStatus.Available;
        }

        await _context.SaveChangesAsync();

        var response = MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.WashBayTimeSlot);
        await _hubContext.Clients.Group(booking.BranchId.ToString())
            .SendAsync("ReceiveBookingCancelled", new { BookingId = booking.Id, Reason = reason });
        return response;
    }

    public async Task<BookingResponse> UpdateBookingAsync(Guid userId, Guid bookingId, UpdateBookingRequest request)
    {
        // 1. Kiểm tra tài khoản Customer
        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        // 2. Kiểm tra đơn Booking hiện tại (Chỉ cho phép sửa khi trạng thái là Pending hoặc Confirmed)
        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.WashBayTimeSlot)
                .ThenInclude(wbts => wbts.TimeSlot)
            .Include(b => b.WashBayTimeSlot)
                .ThenInclude(wbts => wbts.WashBay)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Pending or Confirmed bookings can be modified.", 400);

        // 3. Xác định các thông tin mới (nếu không truyền thì giữ nguyên dữ liệu cũ)
        var targetBranchId = request.BranchId ?? booking.BranchId;
        var targetWashPackageId = request.WashPackageId ?? booking.WashPackageId;
        var targetBookingDate = request.BookingDate ?? booking.WashBayTimeSlot.Date;
        var targetStartTime = request.StartTime ?? booking.WashBayTimeSlot.TimeSlot.StartTime;

        // 4. Kiểm tra xem có bất kỳ sự thay đổi thực sự nào không
        bool isBranchChanged = targetBranchId != booking.BranchId;
        bool isPackageChanged = targetWashPackageId != booking.WashPackageId;
        bool isTimeChanged = targetBookingDate != booking.WashBayTimeSlot.Date || targetStartTime != booking.WashBayTimeSlot.TimeSlot.StartTime;

        if (!isBranchChanged && !isPackageChanged && !isTimeChanged)
        {
            throw new AppException("No changes detected in your update request.", 400);
        }

        // 5. Nếu thay đổi Gói dịch vụ -> Load thông tin gói mới để tính lại giá tiền
        if (isPackageChanged)
        {
            var newPackage = await _context.WashPackages
                .FirstOrDefaultAsync(wp => wp.Id == targetWashPackageId && wp.IsActive)
                ?? throw new AppException("New wash package not found or inactive.", 404);
            
            booking.WashPackageId = newPackage.Id;
            booking.TotalPrice = newPackage.Price; 
        }

        // 6. Nếu có thay đổi Chi nhánh hoặc Thời gian đặt lịch
        if (isBranchChanged || isTimeChanged)
        {
            // Kiểm tra chi nhánh mới có đang hoạt động không
            var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == targetBranchId)
                ?? throw new AppException("Target branch not found.", 404);

            if (branch.Status != BranchStatus.Active)
                throw new AppException("The selected branch is not active.", 400);

            // Kiểm tra thời gian quá khứ dựa trên TimeZone của tiệm
            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _shopTimeZone);
            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            if (targetBookingDate < today)
                throw new AppException("New booking date cannot be in the past.", 400);

            if (targetBookingDate == today && targetStartTime <= nowTime)
                throw new AppException("New start time cannot be in the past.", 400);

            // Kiểm tra giới hạn ngày đặt (Booking Window) theo Tier của khách hàng
            var maxDays = customer.Tier.BookingWindow;
            if (targetBookingDate > today.AddDays(maxDays))
                throw new AppException($"Your tier allows booking up to {maxDays} days in advance.", 400);

            // Đảm bảo thời gian nằm trong khung giờ mở cửa
            if (targetStartTime < _options.OpenTime || targetStartTime >= _options.CloseTime)
                throw new AppException($"Booking time must be within business hours ({_options.OpenTime:HH\\:mm}–{_options.CloseTime:HH\\:mm}).", 400);

            // KIỂM TRA TRÙNG LỊCH CỦA XE (Bỏ qua chính lịch đặt hiện tại)
            var vehicleConflict = await _context.Bookings.AnyAsync(b =>
                b.Id != booking.Id &&
                b.VehicleId == booking.VehicleId &&
                (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.InProgress) &&
                b.WashBayTimeSlot.Date == targetBookingDate &&
                b.WashBayTimeSlot.TimeSlot.StartTime == targetStartTime);

            if (vehicleConflict)
                throw new AppException("This vehicle already has another booking at the selected time.", 409);

            // BẮT ĐẦU TRANSACTION ĐỂ ĐỔI LỊCH (Tránh Race Condition)
            await using var transaction = await _context.BeginTransactionAsync();

            // Tìm ô lịch trống mới
            var newAvailableSlot = await _context.WashBayTimeSlots
                .Include(wbts => wbts.WashBay)
                .Include(wbts => wbts.TimeSlot)
                .FirstOrDefaultAsync(wbts =>
                    wbts.WashBay.BranchId == targetBranchId &&
                    wbts.Date == targetBookingDate &&
                    wbts.TimeSlot.StartTime == targetStartTime &&
                    wbts.Booking == null && 
                    wbts.WashBay.Status == WashBayStatus.Available);

            if (newAvailableSlot == null)
                throw new AppException("No wash bay is available at the selected branch/time slot.", 409);

            // Kiểm tra loại xe hợp lệ với Khoang rửa mới
            var typeName = booking.Vehicle.Type.ToString();
            var supported = newAvailableSlot.WashBay.SupportedTypes
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (supported.Length > 0 && !supported.Any(s => string.Equals(s, typeName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new AppException($"The available wash bay for this slot does not support {typeName} vehicles.", 400);
            }

            // GIẢI PHÓNG Ô LỊCH CŨ
            if (booking.WashBayTimeSlot != null)
            {
                booking.WashBayTimeSlot.Status = TimeSlotStatus.Available;
            }

            // ÁP ĐẶT Ô LỊCH MỚI
            newAvailableSlot.Status = TimeSlotStatus.Booked;
            booking.WashBayTimeSlotId = newAvailableSlot.Id;
            booking.BranchId = targetBranchId;

            booking.UpdatedAt = DateTime.UtcNow;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Load lại đầy đủ thông tin để mapping ra response chính xác nhất
            var updatedBooking = await _context.Bookings
                .Include(b => b.WashPackage)
                .Include(b => b.Vehicle)
                .Include(b => b.WashBayTimeSlot)
                    .ThenInclude(wbts => wbts.TimeSlot)
                .Include(b => b.WashBayTimeSlot)
                    .ThenInclude(wbts => wbts.WashBay)
                .FirstAsync(b => b.Id == booking.Id);

            return MapToResponse(updatedBooking, updatedBooking.WashPackage, updatedBooking.Vehicle, updatedBooking.WashBayTimeSlot);
        }

        // Trường hợp chỉ thay đổi gói dịch vụ mà giữ nguyên giờ/chi nhánh
        booking.UpdatedAt = DateTime.UtcNow;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.WashBayTimeSlot);
    }
    
    public async Task<BookingResponse> CompleteBookingAsync(Guid bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.WashBayTimeSlot)
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status is BookingStatus.Pending or BookingStatus.Cancelled)
            throw new AppException("Only confirmed or in-progress bookings can be completed.", 400);

        if (booking.Status != BookingStatus.Completed)
        {
            booking.Status = BookingStatus.Completed;
            booking.UpdatedAt = DateTime.UtcNow;
            
            if (booking.WashBayTimeSlot != null)
                booking.WashBayTimeSlot.Status = TimeSlotStatus.Completed;
                
            await _context.SaveChangesAsync();
        }

        await _loyaltyService.AwardPointsForBookingAsync(bookingId);

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.WashBayTimeSlot);
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
        Booking booking, WashPackage washPackage, Vehicle vehicle, WashBayTimeSlot? slot)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            WashPackageName = washPackage.Name,
            DurationMinutes = washPackage.DurationMinutes,
            BookingDate = slot?.Date ?? DateOnly.MinValue,
            StartTime = slot?.TimeSlot?.StartTime ?? TimeOnly.MinValue,
            EndTime = slot?.TimeSlot?.StartTime.Add(TimeSpan.FromMinutes(washPackage.DurationMinutes)), // EndTime tính động dựa theo gói rửa
            WashBayName = slot?.WashBay?.Name ?? "N/A",
            VehiclePlate = vehicle.LicensePlate,
            VehicleName = vehicle.VehicleName,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            QrData = booking.QrData,
            CreatedAt = booking.CreatedAt,
        };
    }
    
    private static BookingResponse MapToResponse(
        Booking booking, WashPackage washPackage, Vehicle vehicle, WashBayTimeSlot? slot, Branch branch)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            WashPackageName = washPackage.Name,
            DurationMinutes = washPackage.DurationMinutes,
            BookingDate = slot?.Date ?? DateOnly.MinValue,
            StartTime = slot?.TimeSlot?.StartTime ?? TimeOnly.MinValue,
            EndTime = slot?.TimeSlot?.StartTime.Add(TimeSpan.FromMinutes(washPackage.DurationMinutes)), // EndTime tính động dựa theo gói rửa
            WashBayName = slot?.WashBay?.Name ?? "N/A",
            VehiclePlate = vehicle.LicensePlate,
            VehicleName = vehicle.VehicleName,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            QrData = booking.QrData,
            CreatedAt = booking.CreatedAt,
            BranchId = booking.BranchId,
            BranchName = branch.BranchName
        };
    }
}