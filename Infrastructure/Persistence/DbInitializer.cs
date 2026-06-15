using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var daysToGenerate = 31; 
        
        // --- CHỖ SỬA 1: Tính ngày kết thúc để giới hạn vùng dữ liệu cần bốc lên RAM ---
        var endDate = today.AddDays(daysToGenerate);

        var allWashBayIds = await context.WashBays.Select(wb => wb.Id).ToListAsync();
        var masterTimeSlots = await context.TimeSlots.ToListAsync();
        
        // --- CHỖ SỬA 2: Bốc sạch lịch trong 31 ngày tới lên RAM biến thành chuỗi KeyUnique bỏ vào HashSet ---
        var existingSlotsKeySet = (await context.WashBayTimeSlots
            .Where(wbts => wbts.Date >= today && wbts.Date <= endDate)
            .Select(wbts => $"{wbts.WashBayId}_{wbts.TimeSlotId}_{wbts.Date}")
            .ToListAsync())
            .ToHashSet();

        var seedList = new List<WashBayTimeSlot>();

        for (int i = 0; i < daysToGenerate; i++)
        {
            var currentDate = today.AddDays(i);

            foreach (var bayId in allWashBayIds)
            {
                foreach (var masterSlot in masterTimeSlots)
                {
                    // --- CHỖ SỬA 3: Tạo chuỗi key định danh để so sánh đối chiếu ---
                    var currentKey = $"{bayId}_{masterSlot.Id}_{currentDate}";

                    // --- CHỖ SỬA 4: Thay thế câu lệnh AnyAsync (vả SQL liên tục) bằng lệnh Contains trên RAM ---
                    if (!existingSlotsKeySet.Contains(currentKey))
                    {
                        seedList.Add(new WashBayTimeSlot
                        {
                            Id = Guid.NewGuid(),
                            WashBayId = bayId,
                            TimeSlotId = masterSlot.Id,
                            Date = currentDate,
                            Status = TimeSlotStatus.Available
                        });
                    }
                }
            }
        }

        if (seedList.Any())
        {
            await context.WashBayTimeSlots.AddRangeAsync(seedList);
            await context.SaveChangesAsync();
        }
    }
}