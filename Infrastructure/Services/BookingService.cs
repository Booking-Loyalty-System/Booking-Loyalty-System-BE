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
using Npgsql;
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

    // Resolve optional add-ons. Every requested id must map to an active add-on.
    var requestedAddOnIds = request.AddOnIds?.Distinct().ToList() ?? new List<Guid>();
    var addOns = requestedAddOnIds.Count == 0
        ? new List<AddOn>()
        : await _context.AddOns
            .Where(a => requestedAddOnIds.Contains(a.Id) && a.IsActive)
            .ToListAsync();
    if (addOns.Count != requestedAddOnIds.Count)
        throw new AppException("One or more add-ons not found or inactive.", 400);

    var addOnsTotal = addOns.Sum(a => a.Price);

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

    // 4. Chống ôm slot: giới hạn số booking Pending (chưa được staff xác nhận) mỗi khách.
    //    Không có rào thanh toán nên đây là biện pháp thay cho "đặt cọc" để chặn giữ chỗ tràn lan.
    var activePending = await _context.Bookings
        .CountAsync(b => b.CustomerId == customer.Id && b.Status == BookingStatus.Pending);
    if (activePending >= _options.MaxActivePendingBookings)
        throw new AppException(
            $"You already have {activePending} booking(s) awaiting staff confirmation. " +
            "Please wait until they are confirmed before booking more.", 409);

    // 5. Mở TRANSACTION (Serializable) chống Race Condition khi đặt chỗ cùng giây.
    //    Cả check trùng xe lẫn đếm capacity đều nằm TRONG transaction để không lọt khi đặt đồng thời.
    await using var transaction = await _context.BeginTransactionAsync();

    // 5a. Chặn trùng lịch của chiếc xe này trong cùng khung giờ.
    var vehicleConflict = await _context.Bookings.AnyAsync(b =>
        b.VehicleId == vehicle.Id &&
        b.BookingDate == request.BookingDate &&
        b.BranchTimeSlot.TimeSlotId == timeSlot.Id &&
        (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.InProgress));

    if (vehicleConflict)
        throw new AppException("This vehicle already has a booking that overlaps the selected time.", 409);

    // 5b. Đếm số xe đang chiếm chỗ (Bỏ Cancelled và NoShow).
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
    Guid? rewardId = null;
    string? voucherName = null;
    RewardRedemption? appliedRedemption = null;

    // Cho phép dùng ĐỒNG THỜI promotion + voucher: promotion (giảm %) tính TRƯỚC trên giá gói rửa,
    // rồi voucher (giảm tiền cố định) trừ TIẾP trên phần còn lại. Giá sau giảm không bao giờ âm.
    if (!string.IsNullOrWhiteSpace(request.PromotionCode))
    {
        // Truyền customer + branch để enforce điều kiện sinh nhật / hạng / chi nhánh (địa chỉ).
        var (pid, discount) = await _promotionService.ApplyAsync(request.PromotionCode, washPackage.Price, customer, request.BranchId);
        promotionId = pid;
        discountAmount += discount;
        totalPrice -= discount;
    }

    if (request.RewardRedemptionId.HasValue)
    {
        // Voucher đổi bằng điểm (giảm số tiền cố định) — trừ trên phần GIÁ CÒN LẠI sau promotion.
        appliedRedemption = await _context.RewardRedemptions
            .Include(r => r.Reward)
            .FirstOrDefaultAsync(r => r.Id == request.RewardRedemptionId.Value
                && r.CustomerId == customer.Id
                && r.Status == RedemptionStatus.Pending
                && (r.ExpiryDate == null || r.ExpiryDate > DateTime.UtcNow))
            ?? throw new AppException("Voucher not found, already used, or expired.", 400);

        var voucherDiscount = Math.Min(appliedRedemption.Reward.DiscountAmount, totalPrice);
        discountAmount += voucherDiscount;
        totalPrice -= voucherDiscount;
        rewardId = appliedRedemption.RewardId;
        voucherName = appliedRedemption.Reward.Name;
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
        RewardId = rewardId,
        // Vouchers/promotions discount the wash package only; add-ons are added on top.
        TotalPrice = totalPrice + addOnsTotal,
        StartTime = branchTimeSlot.TimeSlot.StartTime,
        DiscountAmount = discountAmount,
        Status = BookingStatus.Pending,
        CreatedAt = DateTime.UtcNow
    };

    _context.Bookings.Add(booking);

    // Consume the voucher: mark its redemption fulfilled and link it to this booking.
    if (appliedRedemption != null)
    {
        appliedRedemption.Status = RedemptionStatus.Fulfilled;
        appliedRedemption.FulfilledAt = DateTime.UtcNow;
        appliedRedemption.BookingId = booking.Id;
    }

    // Snapshot the chosen add-ons onto the booking (price frozen at booking time).
    var bookingAddOns = addOns.Select(a => new BookingAddOn
    {
        Id = Guid.NewGuid(),
        BookingId = booking.Id,
        AddOnId = a.Id,
        Price = a.Price,
        DurationMinutes = a.DurationMinutes,
        CreatedAt = DateTime.UtcNow
    }).ToList();
    if (bookingAddOns.Count > 0)
        _context.BookingAddOns.AddRange(bookingAddOns);

    try
    {
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    catch (Exception ex) when (IsSerializationConflict(ex))
    {
        // Hai khách giành suất cuối cùng cùng lúc: transaction Serializable thua bị DB từ chối (40001).
        // Trả 409 sạch thay vì để lỗi nổ thành 500; `await using` sẽ tự rollback khi thoát method.
        throw new AppException("This time slot was just taken by another customer. Please try again.", 409);
    }

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
    var response = MapToResponse(booking, washPackage, vehicle, timeSlot, branch, null, voucherName);
    response.AddOns = addOns
        .Select(a => new BookingAddOnResponse { AddOnId = a.Id, Name = a.Name, Price = a.Price })
        .ToList();

    await _hubContext.Clients.Group(branchTimeSlot.BranchId.ToString())
        .SendAsync("ReceiveBookingCreated", response);

    return response;
}

    /// <summary>
    /// True if the exception (or an inner one) is a PostgreSQL serialization/deadlock failure
    /// (SQLSTATE 40001 / 40P01) — i.e. two concurrent bookings raced for the same last slot.
    /// </summary>
    private static bool IsSerializationConflict(Exception? ex)
    {
        for (var e = ex; e != null; e = e.InnerException)
            if (e is PostgresException pg && (pg.SqlState == "40001" || pg.SqlState == "40P01"))
                return true;
        return false;
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

        // Hủy booking thì nhả voucher đã áp về Pending để khách dùng lại (voucher bị "tiêu"
        // ngay lúc tạo booking Pending; nếu không hoàn thì hủy là mất oan).
        await ReleaseVoucherForBookingAsync(booking.Id);

        await _context.SaveChangesAsync();

        var response = MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.BranchTimeSlot.TimeSlot, booking.BranchTimeSlot.Branch, booking.WashBay);
        
        await _hubContext.Clients.Group(booking.BranchTimeSlot.BranchId.ToString())
            .SendAsync("ReceiveBookingCancelled", new { BookingId = booking.Id, Reason = reason });

        return response;
    }

    /// <summary>
    /// Returns a voucher that was applied to a now-cancelled booking back to the customer's
    /// wallet (Fulfilled -> Pending), so it becomes usable again. No-op if the booking had no
    /// voucher. The caller is responsible for SaveChanges. Not used for No-show (forfeited).
    /// </summary>
    private async Task ReleaseVoucherForBookingAsync(Guid bookingId)
    {
        var redemption = await _context.RewardRedemptions
            .FirstOrDefaultAsync(r => r.BookingId == bookingId
                && r.Status == RedemptionStatus.Fulfilled);
        if (redemption == null)
            return;

        redemption.Status = RedemptionStatus.Pending;
        redemption.FulfilledAt = null;
        redemption.BookingId = null;
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
        Booking booking, WashPackage washPackage, Vehicle vehicle, TimeSlot timeSlot, Branch branch, WashBay? washBay, string? voucherName = null)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            WashPackageName = washPackage.Name,
            DurationMinutes = washPackage.DurationMinutes,
            BookingDate = booking.BookingDate,
            StartTime = timeSlot.StartTime,
            EndTime = timeSlot.StartTime.Add(TimeSpan.FromMinutes(washPackage.DurationMinutes)),
            WashBayName = washBay?.Name ?? "Not Assigned Yet",
            VehiclePlate = vehicle.LicensePlate,
            VehicleName = vehicle.VehicleName,
            TotalPrice = booking.TotalPrice,
            DiscountAmount = booking.DiscountAmount,
            VoucherName = voucherName,
            Status = booking.Status.ToString(),
            QrData = booking.QrData,
            CreatedAt = booking.CreatedAt,
            BranchId = branch.Id,
            BranchName = branch.BranchName
        };
    }
}