using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Persistence;

public static class DbInitializer
{
    private static Guid CreateDeterministicGuid(string src)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(src);
        byte[] hashBytes = MD5.HashData(inputBytes);
        return new Guid(hashBytes);
    }

    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // 1. Lấy danh sách Chi nhánh và Khung giờ
        var branches = await context.Branches.ToListAsync();
        var masterTimeSlots = await context.TimeSlots.ToListAsync();

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
                var deterministicId = CreateDeterministicGuid($"{branch.Id}_{masterSlot.Id}");
                // 4. Nếu Chi nhánh này chưa có cấu hình cho khung giờ này thì thêm mới
                if (!existingBranchTimeSlotsKeySet.Contains(currentKey))
                {
                    seedList.Add(new BranchTimeSlot
                    {
                        Id = deterministicId,
                        BranchId = branch.Id,
                        TimeSlotId = masterSlot.Id,
                        MaxCapacity = defaultCapacity,
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

        var freeWashRewardId = Guid.Parse("10000000-0000-0000-0000-000000000099");
        if (!await context.Rewards.AnyAsync(r => r.Id == freeWashRewardId))
        {
            context.Rewards.Add(new Reward
            {
                Id = freeWashRewardId,
                Code = "FREEWASH_GIFT",
                Name = "Voucher Rửa Xe Miễn Phí",
                Description = "Quà tặng khi hoàn thành 7 lượt rửa trong chu kỳ.",
                PointsCost = 0,
                PointsRequired = 0,
                DiscountAmount = 200000.00m, // Phủ trọn giá gói rửa đắt nhất → rửa miễn phí (BookingService dùng Min(DiscountAmount, totalPrice)).
                IsFreeWash = true,
                IsActive = true,
                Status = true,
                StartDate = new DateOnly(2026, 1, 1),
                EndDate = new DateOnly(2030, 12, 31)
            });
            await context.SaveChangesAsync();
        }

        // Seed Booking

        if (!await context.Bookings.AnyAsync())
        {
            var bookings = new List<Booking>();

            var customerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var vehicleId = Guid.Parse("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128");
            var washPackageId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002");
            var bayId = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000001");

            int globalCounter = 1;
            var startDate = new DateOnly(2025, 1, 1);
            var endDate = new DateOnly(2025, 12, 31);

            // Danh sách cấu trúc Branch và Staff
            var branchDataMatrix = new[]
            {
                new {
                    BranchId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02",
                    StaffId = "11111111-1111-1111-1111-111111111113"
                },
                new {
                    BranchId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03",
                    StaffId = "11111111-1111-1111-1111-111111111112"
                },
                new {
                    BranchId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
                    StaffId = "11111111-1111-1111-1111-111111111111"
                }
            };

            var startTimes = new List<TimeOnly>
            {
                new TimeOnly(8, 0),  new TimeOnly(9, 0),  new TimeOnly(10, 0), new TimeOnly(11, 0),
                new TimeOnly(12, 0), new TimeOnly(13, 0), new TimeOnly(14, 0), new TimeOnly(15, 0),
                new TimeOnly(16, 0), new TimeOnly(17, 0)
            };

            // Lấy danh sách master slots từ DB để biết chính xác cấu trúc tạo ID
            var slotIdsInDb = masterTimeSlots.Select(s => s.Id.ToString()).ToList();

            for (var currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
            {
                for (int branchIdx = 0; branchIdx < branchDataMatrix.Length; branchIdx++)
                {
                    var currentBranch = branchDataMatrix[branchIdx];

                    bool isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;
                    int dailyBookingCount = isWeekend ? 2 : (currentDate.Day % 3 == 0 ? 1 : 2);

                    for (int b = 0; b < dailyBookingCount; b++)
                    {
                        int slotIndex = (currentDate.DayOfYear + branchIdx * 3 + b * 2) % slotIdsInDb.Count;

                        // Tái tạo lại chính xác ID cố định của BranchTimeSlot dựa theo logic sinh MD5 ở trên
                        var masterSlotId = slotIdsInDb[slotIndex];
                        var branchTimeSlotGuidId = CreateDeterministicGuid($"{currentBranch.BranchId}_{masterSlotId}");

                        var startTime = startTimes[slotIndex % startTimes.Count];

                        string bookingCode = $"BK25{globalCounter:D5}";
                        var bookingGuid = Guid.Parse($"25252525-2525-2525-2525-{globalCounter:D12}");

                        decimal totalPrice = 120000.00m + (slotIndex * 15000m);

                        var createdAt = new DateTime(2025, currentDate.Month, currentDate.Day, 6, 0, 0, DateTimeKind.Utc).AddDays(-1);
                        var updatedAt = new DateTime(2025, currentDate.Month, currentDate.Day, startTime.Hour, startTime.Minute, 0, DateTimeKind.Utc).AddHours(1);

                        bookings.Add(new Booking
                        {
                            Id = bookingGuid,
                            BookingCode = bookingCode,
                            Status = BookingStatus.CheckedOut,
                            TotalPrice = totalPrice,
                            DiscountAmount = 0.00m,
                            BranchTimeSlotId = branchTimeSlotGuidId, // Map chuẩn vào ID cố định
                            BayId = bayId,
                            CustomerId = customerId,
                            VehicleId = vehicleId,
                            WashPackageId = washPackageId,
                            StaffId = Guid.Parse(currentBranch.StaffId),
                            BookingDate = currentDate,
                            StartTime = startTime,
                            CreatedAt = createdAt,
                            UpdatedAt = updatedAt,
                            CustomerNote = $"Data seed doanh thu hệ thống năm 2025 - Ngày {currentDate:dd/MM/yyyy}"
                        });

                        globalCounter++;
                    }
                }
            }

            // Tiến hành lưu hàng loạt
            try
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                const int batchSize = 1000;
                for (int i = 0; i < bookings.Count; i += batchSize)
                {
                    var batch = bookings.Skip(i).Take(batchSize);
                    await context.Bookings.AddRangeAsync(batch);
                    await context.SaveChangesAsync();
                }
            }
            finally
            {
                context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        var hasSeed2026 = await context.Bookings.AnyAsync(b => b.BookingDate >= new DateOnly(2026, 1, 1));

        if (!hasSeed2026)
        {
            var bookings2026 = new List<Booking>();

            var customerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var vehicleId = Guid.Parse("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128");
            var washPackageId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002");
            var bayId = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000001");

            // Đếm số lượng booking hiện tại từ năm 2025 để cộng dồn counter, tránh trùng mã code
            int currentTotalBookings = await context.Bookings.CountAsync();
            int globalCounter = currentTotalBookings + 1;

            // Cấu hình mốc thời gian từ đầu năm 2026 đến ngày hôm nay
            var startDate2026 = new DateOnly(2026, 1, 1);
            var endDate2026 = DateOnly.FromDateTime(DateTime.UtcNow);

            var branchDataMatrix = new[]
            {
        new {
            BranchId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02",
            StaffId = "11111111-1111-1111-1111-111111111113"
        },
        new {
            BranchId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03",
            StaffId = "11111111-1111-1111-1111-111111111112"
        },
        new {
            BranchId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
            StaffId = "11111111-1111-1111-1111-111111111111"
        }
    };

            var startTimes = new List<TimeOnly>
    {
        new TimeOnly(8, 0),  new TimeOnly(9, 0),  new TimeOnly(10, 0), new TimeOnly(11, 0),
        new TimeOnly(12, 0), new TimeOnly(13, 0), new TimeOnly(14, 0), new TimeOnly(15, 0),
        new TimeOnly(16, 0), new TimeOnly(17, 0)
    };

            var slotIdsInDb = masterTimeSlots.Select(s => s.Id.ToString()).ToList();

            for (var currentDate = startDate2026; currentDate <= endDate2026; currentDate = currentDate.AddDays(1))
            {
                for (int branchIdx = 0; branchIdx < branchDataMatrix.Length; branchIdx++)
                {
                    var currentBranch = branchDataMatrix[branchIdx];

                    bool isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;
                    int dailyBookingCount = isWeekend ? 2 : (currentDate.Day % 3 == 0 ? 1 : 2);

                    for (int b = 0; b < dailyBookingCount; b++)
                    {
                        int slotIndex = (currentDate.DayOfYear + branchIdx * 3 + b * 2) % slotIdsInDb.Count;

                        var masterSlotId = slotIdsInDb[slotIndex];
                        var branchTimeSlotGuidId = CreateDeterministicGuid($"{currentBranch.BranchId}_{masterSlotId}");

                        var startTime = startTimes[slotIndex % startTimes.Count];

                        // Sinh mã dạng BK26xxxxx nối tiếp số counter toàn cục
                        string bookingCode = $"BK26{globalCounter:D5}";

                        // Sinh GUID dạng 26262626-... để phân biệt rõ với dữ liệu 2025 cũ
                        var bookingGuid = Guid.Parse($"26262626-2626-2626-2626-{globalCounter:D12}");

                        decimal totalPrice = 120000.00m + (slotIndex * 15000m);

                        var createdAt = new DateTime(2026, currentDate.Month, currentDate.Day, 6, 0, 0, DateTimeKind.Utc).AddDays(-1);
                        var updatedAt = new DateTime(2026, currentDate.Month, currentDate.Day, startTime.Hour, startTime.Minute, 0, DateTimeKind.Utc).AddHours(1);

                        bookings2026.Add(new Booking
                        {
                            Id = bookingGuid,
                            BookingCode = bookingCode,
                            Status = BookingStatus.CheckedOut,
                            TotalPrice = totalPrice,
                            DiscountAmount = 0.00m,
                            BranchTimeSlotId = branchTimeSlotGuidId,
                            BayId = bayId,
                            CustomerId = customerId,
                            VehicleId = vehicleId,
                            WashPackageId = washPackageId,
                            StaffId = Guid.Parse(currentBranch.StaffId),
                            BookingDate = currentDate,
                            StartTime = startTime,
                            CreatedAt = createdAt,
                            UpdatedAt = updatedAt,
                            CustomerNote = $"Data seed doanh thu hệ thống năm 2026 - Ngày {currentDate:dd/MM/yyyy}"
                        });

                        globalCounter++;
                    }
                }
            }

            // Tiến hành lưu hàng loạt dữ liệu 2026
            try
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                const int batchSize = 1000;
                for (int i = 0; i < bookings2026.Count; i += batchSize)
                {
                    var batch = bookings2026.Skip(i).Take(batchSize);
                    await context.Bookings.AddRangeAsync(batch);
                    await context.SaveChangesAsync();
                }
            }
            finally
            {
                context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        var bookings2026WithoutFeedback = await context.Bookings
            .Where(b => b.BookingDate >= new DateOnly(2026, 1, 1) && b.Feedback == null)
            .Select(b => new { b.Id, b.CustomerId, b.BookingDate, b.StartTime })
            .ToListAsync();

        if (bookings2026WithoutFeedback.Any())
        {
            var seedFeedbacks = new List<Feedback>();

            // Mẫu comment phong phú để seed cho tự nhiên
            var sampleComments = new[]
            {
                "Dịch vụ rất tốt, rửa sạch sẽ và cẩn thận!",
                "Nhân viên nhiệt tình, phòng chờ mát mẻ.",
                "Giá cả hợp lý, sẽ tiếp tục ủng hộ chi nhánh.",
                "Rửa xe khá nhanh, đúng giờ đã đặt.",
                "Chất lượng dịch vụ tuyệt vời, 5 sao!",
                "Xe sạch bong kin kịt, lau khô rất kỹ.",
                "Khá hài lòng với chất lượng phục vụ ở đây.",
                "Dịch vụ tốt, lần sau sẽ quay lại."
            };

            int feedbackCounter = 1;

            foreach (var bk in bookings2026WithoutFeedback)
            {
                // Chọn ngẫu nhiên/xoay vòng điểm đánh giá (chủ yếu từ 4 đến 5 sao)
                int staffRating = (feedbackCounter % 5 == 0) ? 4 : 5;
                int serviceRating = (feedbackCounter % 7 == 0) ? 4 : 5;
                int priceRating = (feedbackCounter % 3 == 0) ? 4 : 5;

                // Điểm trung bình cộng
                double overallRating = Math.Round((staffRating + serviceRating + priceRating) / 3.0, 1);

                // Lấy mẫu comment
                string comment = sampleComments[feedbackCounter % sampleComments.Length];

                // Thời gian tạo feedback: khoảng 1-2 tiếng sau khi hoàn thành dịch vụ
                var feedbackCreatedAt = new DateTime(
                    bk.BookingDate.Year,
                    bk.BookingDate.Month,
                    bk.BookingDate.Day,
                    bk.StartTime.Hour,
                    bk.StartTime.Minute,
                    0,
                    DateTimeKind.Utc
                ).AddHours(2);

                // ID Deterministic dạng 36363636-... cho Feedback
                var feedbackGuid = Guid.Parse($"36363636-3636-3636-3636-{feedbackCounter:D12}");

                seedFeedbacks.Add(new Feedback
                {
                    Id = feedbackGuid,
                    BookingId = bk.Id,
                    CustomerId = bk.CustomerId,
                    StaffRating = staffRating,
                    ServiceRating = serviceRating,
                    PriceRating = priceRating,
                    Comment = comment,
                    CreatedAt = feedbackCreatedAt
                });

                feedbackCounter++;
            }

            try
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                const int batchSize = 1000;
                for (int i = 0; i < seedFeedbacks.Count; i += batchSize)
                {
                    var batch = seedFeedbacks.Skip(i).Take(batchSize);
                    await context.Feedbacks.AddRangeAsync(batch);
                    await context.SaveChangesAsync();
                }
            }
            finally
            {
                context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}