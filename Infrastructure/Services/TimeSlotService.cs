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

        // 1. Lấy tổng số khoang rửa thực tế mà chi nhánh này đang sở hữu làm mốc tính toán gốc
        int totalBaysInBranch = await _context.WashBays
            .CountAsync(wb => wb.BranchId == branchId);

        // 2. Lấy 11 mốc giờ master cố định (08h - 18h)
        var masterTimeSlots = await _context.TimeSlots
            .OrderBy(ts => ts.StartTime)
            .ToListAsync();

        // 3. Lấy toàn bộ ô lịch gối đầu thuộc về riêng chi nhánh trong 7 ngày
        var branchSlotsInWeek = await _context.WashBayTimeSlots
            .Include(wbts => wbts.WashBay)
            .Where(wbts => wbts.WashBay.BranchId == branchId 
                        && wbts.Date >= startDate 
                        && wbts.Date <= endDate)
            .ToListAsync();

        var weeklySummary = new List<DailyTimeSlotsSummaryResponse>();

        for (int i = 0; i < 7; i++)
        {
            var currentDate = startDate.AddDays(i);

            // Lọc dữ liệu thuộc ngày hiện tại trong vòng lặp
            var slotsInCurrentDay = branchSlotsInWeek.Where(wbts => wbts.Date == currentDate).ToList();

            var dailySlots = masterTimeSlots.Select(ts =>
            {
                // Tìm xem ca này trong DB đã được tạo lịch gối đầu chưa
                var baysInThisSlot = slotsInCurrentDay.Where(wbts => wbts.TimeSlotId == ts.Id).ToList();

                int availableBays = 0;

                // 🔥 LOGIC VỆ SĨ ĐẬP TAN "FULL" ẢO:
                // Nếu trong DB chưa có dòng dữ liệu gối đầu nào cho ca này (do lệch ngày hoặc chưa seed kịp)
                // Thì mặc định số khoang trống PHẢI BẰNG tổng số khoang của chi nhánh (Hệ thống sạch, chưa ai đặt)
                if (baysInThisSlot.Count == 0)
                {
                    availableBays = totalBaysInBranch;
                }
                else
                {
                    // Nếu đã có dữ liệu gối đầu, đếm chuẩn số lượng khoang đang rảnh (Available)
                    availableBays = baysInThisSlot.Count(wbts => wbts.Status == TimeSlotStatus.Available);
                }

                bool isAvailable = availableBays > 0;
                string slotRatio = isAvailable ? $"{availableBays} slots left" : "Full";

                return new BranchTimeSlotGroupResponse
                {
                    TimeSlotId = ts.Id,
                    StartTime = ts.StartTime,
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