using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // 1. Lấy danh sách Chi nhánh và Khung giờ
        var branches = await context.Branches.ToListAsync();
        var masterTimeSlots = await context.TimeSlots.ToListAsync();

        // 2. Lấy danh sách các cấu hình đã tồn tại trên RAM để đối chiếu (tránh vả DB liên tục)
        var existingBranchTimeSlotsKeySet = (await context.BranchTimeSlots
            .Select(bts => $"{bts.BranchId}_{bts.TimeSlotId}")
            .ToListAsync())
            .ToHashSet();

        var seedList = new List<BranchTimeSlot>();

        // 3. Quét từng chi nhánh và từng khung giờ để tạo cấu hình
        foreach (var branch in branches)
        {
            // Lấy số lượng WashBay thực tế của Chi nhánh để làm MaxCapacity mặc định
            var washBayCount = await context.WashBays.CountAsync(wb => wb.BranchId == branch.Id);
            var defaultCapacity = washBayCount > 0 ? washBayCount : 1; 

            foreach (var masterSlot in masterTimeSlots)
            {
                var currentKey = $"{branch.Id}_{masterSlot.Id}";

                // 4. Nếu Chi nhánh này chưa có cấu hình cho khung giờ này thì thêm mới
                if (!existingBranchTimeSlotsKeySet.Contains(currentKey))
                {
                    seedList.Add(new BranchTimeSlot
                    {
                        Id = Guid.NewGuid(),
                        BranchId = branch.Id,
                        TimeSlotId = masterSlot.Id,
                        MaxCapacity = defaultCapacity, // Sức chứa bằng số bệ rửa
                        IsActive = true
                    });
                }
            }
        }

        // 5. Lưu vào Database
        if (seedList.Any())
        {
            await context.BranchTimeSlots.AddRangeAsync(seedList);
            await context.SaveChangesAsync();
        }
    }
}