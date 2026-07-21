using Application.DTOs.TimeSlot;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TimeSlotService : ITimeSlotService
{
    private readonly IApplicationDbContext _context;

    public TimeSlotService(IApplicationDbContext context)
    {
        _context = context;
    }

   public async Task<List<DailyTimeSlotsSummaryResponse>> GetWeeklySlotsSummaryAsync(Guid branchId, DateOnly startDate)
    {
        var endDate = startDate.AddDays(6);

        // 1. Lấy danh sách cấu hình khung giờ cố định của chi nhánh này
        var branchTimeSlots = await _context.BranchTimeSlots
            .Include(bts => bts.TimeSlot)
            .Where(bts => bts.BranchId == branchId && bts.IsActive)
            .OrderBy(bts => bts.TimeSlot.StartTime)
            .ToListAsync();

        // 2. FIX TẠI ĐÂY: Nhờ DATABASE đếm và gom nhóm hộ luôn, gọn nhẹ và chuẩn xác 100%
        var bookedCounts = await _context.Bookings
            .Where(b => b.BranchTimeSlot.BranchId == branchId 
                     && b.BookingDate >= startDate 
                     && b.BookingDate <= endDate 
                     && b.Status != BookingStatus.Cancelled 
                     && b.Status != BookingStatus.NoShow)
            .GroupBy(b => new { b.BookingDate, b.BranchTimeSlotId })
            .Select(g => new 
            {
                g.Key.BookingDate,
                g.Key.BranchTimeSlotId,
                Count = g.Count()
            })
            .ToListAsync();

        // 3. Biến dữ liệu đã đếm thành Dictionary dạng Tuple (Ngày, ID_Khung_Giờ) để tìm kiếm với tốc độ O(1)
        var bookedDict = bookedCounts.ToDictionary(
            x => (x.BookingDate, x.BranchTimeSlotId), 
            x => x.Count
        );

        var weeklySummary = new List<DailyTimeSlotsSummaryResponse>();

        for (int i = 0; i < 7; i++)
        {
            var currentDate = startDate.AddDays(i);

            var dailySlots = branchTimeSlots.Select(bts =>
            {
                // 4. Check xem trong từ điển hôm nay, khung giờ này có ai đặt chưa. Không có thì mặc định bằng 0.
                int bookedCount = bookedDict.TryGetValue((currentDate, bts.Id), out var count) ? count : 0;
                
                // Toán học cơ bản: Số bệ trống = Sức chứa tối đa - Số xe đã đặt
                int availableBays = bts.MaxCapacity - bookedCount;
                if (availableBays < 0) availableBays = 0; 

                bool isAvailable = availableBays > 0;
                string slotRatio = isAvailable ? $"{availableBays} slots left" : "Full";

                return new BranchTimeSlotGroupResponse
                {
                    TimeSlotId = bts.TimeSlotId,
                    StartTime = bts.TimeSlot.StartTime,
                    SlotRatio = slotRatio,
                    IsAvailable = isAvailable
                };
            }).ToList();

            weeklySummary.Add(new DailyTimeSlotsSummaryResponse
            {
                Date = currentDate,
                DayOfWeek = currentDate.DayOfWeek.ToString(),
                TimeSlots = dailySlots
            });
        }

        return weeklySummary;
    }
}