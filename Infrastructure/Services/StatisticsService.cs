using Application.DTOs.Statistics;
using Application.DTOs.Tier;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IApplicationDbContext _context;
    private static readonly BookingStatus[] CompletedStatuses = { BookingStatus.Completed, BookingStatus.CheckedOut };

    public StatisticsService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OverviewResponse> GetOverviewAsync()
    {
        var totalBookings = await _context.Bookings.CountAsync();
        var completedBookings = await _context.Bookings.CountAsync(b => CompletedStatuses.Contains(b.Status));
        var cancelledBookings = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Cancelled);
        var noShowBookings = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.NoShow);

        var totalRevenue = completedBookings > 0
            ? await _context.Bookings
                .Where(b => CompletedStatuses.Contains(b.Status))
                .SumAsync(b => b.TotalPrice)
            : 0m;

        var totalCustomers = await _context.Customers.CountAsync();

        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var newCustomersThisMonth = await _context.Customers
            .CountAsync(c => c.CreatedAt >= startOfMonth);

        return new OverviewResponse
        {
            TotalRevenue = totalRevenue,
            TotalBookings = totalBookings,
            TotalCustomers = totalCustomers,
            CompletedBookings = completedBookings,
            CancelledBookings = cancelledBookings,
            NoShowBookings = noShowBookings,
            NewCustomersThisMonth = newCustomersThisMonth,
            AvgRevenuePerBooking = completedBookings > 0 ? Math.Round(totalRevenue / completedBookings, 2) : 0
        };
    }

    public async Task<List<RevenueDataPoint>> GetRevenueAsync(DateOnly from, DateOnly to, string groupBy)
    {
        var bookings = await _context.Bookings
            .Where(b => CompletedStatuses.Contains(b.Status)
                && b.BookingDate >= from && b.BookingDate <= to)
            .Select(b => new BookingDatePrice { BookingDate = b.BookingDate, TotalPrice = b.TotalPrice })
            .ToListAsync();

        return GroupByPeriod(bookings, groupBy);
    }

    public async Task<BookingStatsResponse> GetBookingStatsAsync(DateOnly from, DateOnly to, string groupBy)
    {
        var bookings = await _context.Bookings
            .Where(b => b.BookingDate >= from && b.BookingDate <= to)
            .Select(b => new { b.BookingDate, b.TotalPrice, b.Status })
            .ToListAsync();

        var dataPoints = GroupByPeriod(
            bookings.Select(b => new BookingDatePrice { BookingDate = b.BookingDate, TotalPrice = b.TotalPrice }).ToList(),
            groupBy);

        var statusSummary = bookings
            .GroupBy(b => b.Status)
            .Select(g => new BookingStatusSummary { Status = g.Key.ToString(), Count = g.Count() })
            .OrderByDescending(s => s.Count)
            .ToList();

        return new BookingStatsResponse { DataPoints = dataPoints, StatusSummary = statusSummary };
    }

    public async Task<List<TopCustomerResponse>> GetTopCustomersAsync(int top)
    {
        return await _context.Customers
            .Include(c => c.Tier)
            .OrderByDescending(c => c.TotalSpent)
            .Take(top)
            .Select(c => new TopCustomerResponse
            {
                CustomerId = c.Id,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                TotalSpent = c.TotalSpent,
                TotalWashes = c.TotalWashes,
                Tier = c.Tier.TierName
            })
            .ToListAsync();
    }

    public async Task<List<BranchPerformanceResponse>> GetBranchPerformanceAsync(DateOnly from, DateOnly to)
    {
        var bookings = await _context.Bookings
            .Where(b => b.BookingDate >= from && b.BookingDate <= to)
            .Select(b => new
            {
                BranchId = b.BranchTimeSlot.BranchId,
                BranchName = b.BranchTimeSlot.Branch.BranchName,
                b.TotalPrice,
                b.Status
            })
            .ToListAsync();

        return bookings
            .GroupBy(b => new { b.BranchId, b.BranchName })
            .Select(g => new BranchPerformanceResponse
            {
                BranchId = g.Key.BranchId,
                BranchName = g.Key.BranchName,
                TotalRevenue = g.Where(b => CompletedStatuses.Contains(b.Status)).Sum(b => b.TotalPrice),
                TotalBookings = g.Count(),
                CompletedBookings = g.Count(b => CompletedStatuses.Contains(b.Status)),
                CancelledBookings = g.Count(b => b.Status == BookingStatus.Cancelled)
            })
            .OrderByDescending(b => b.TotalRevenue)
            .ToList();
    }

    public async Task<List<TierDistributionResponse>> GetTierDistributionAsync()
    {
        var totalCustomers = await _context.Customers.CountAsync();
        if (totalCustomers == 0)
            return new List<TierDistributionResponse>();

        var groups = await _context.Customers
            .Include(c => c.Tier)
            .GroupBy(c => c.Tier.TierName)
            .Select(g => new { TierName = g.Key, Count = g.Count() })
            .ToListAsync();

        return groups
            .Select(g => new TierDistributionResponse
            {
                TierName = g.TierName,
                CustomerCount = g.Count,
                Percentage = Math.Round((decimal)g.Count / totalCustomers * 100, 1)
            })
            .OrderByDescending(t => t.CustomerCount)
            .ToList();
    }

    public async Task<List<TierPeriodReportResponse>> GetTierStatisticsAsync(TierStatisticsRequest request)
    {
        // 1. Lấy toàn bộ danh sách khách hàng và thông tin hạng của họ
        // LƯU Ý: Ở đây đang dùng Customer.CreatedAt để demo theo chu kỳ đăng ký/hoạt động.
        // Nếu sau này bạn có bảng lịch sử nâng hạng, hãy query từ bảng đó.
        var query = _context.Customers
            .Include(c => c.Tier)
            .Where(c => c.CreatedAt.Year == request.Year);

        // 2. Nhóm dữ liệu theo tiêu chí (Tháng, Quý, Năm)
        var groupedData = await query.ToListAsync();

        var rawPeriods = groupedData
            .GroupBy(c => new
            {
                PeriodNumber = request.Criteria.ToLower() switch
                {
                    "month" => c.CreatedAt.Month,
                    "quarter" => (c.CreatedAt.Month - 1) / 3 + 1,
                    _ => 1 // "year"
                },
                PeriodLabel = request.Criteria.ToLower() switch
                {
                    "month" => $"Tháng {c.CreatedAt.Month:D2}/{request.Year}",
                    "quarter" => $"Quý {(c.CreatedAt.Month - 1) / 3 + 1}/{request.Year}",
                    _ => $"Năm {request.Year}"
                }
            })
            .OrderBy(g => g.Key.PeriodNumber)
            .ToList();

        // Lấy danh sách tất cả các Tier hiện có để đảm bảo kỳ nào cũng hiển thị đủ các hạng (dù count = 0)
        var allTiers = await _context.Tiers.ToListAsync();

        var report = new List<TierPeriodReportResponse>();
        TierPeriodReportResponse previousPeriodReport = null;

        // 3. Tính toán số lượng, tỉ lệ % và độ chênh lệch
        foreach (var group in rawPeriods)
        {
            var periodReport = new TierPeriodReportResponse()
            {
                PeriodLabel = group.Key.PeriodLabel,
                TotalCustomersInPeriod = group.Count()
            };

            foreach (var tier in allTiers)
            {
                var customersInTier = group.Where(c => c.TierId == tier.Id).ToList();
                int count = customersInTier.Count;

                // Tính % của hạng này chiếm bao nhiêu trong tổng số khách của kỳ hiện tại
                double percentage = periodReport.TotalCustomersInPeriod > 0
                    ? Math.Round((double)count / periodReport.TotalCustomersInPeriod * 100, 2)
                    : 0;

                // Tính % chênh lệch số lượng so với kỳ trước đó
                double percentageChange = 0;
                if (previousPeriodReport != null)
                {
                    var prevTierCount = previousPeriodReport.Tiers.FirstOrDefault(t => t.TierId == tier.Id)?.Count ?? 0;
                    if (prevTierCount > 0)
                    {
                        // Công thức: ((Kỳ này - Kỳ trước) / Kỳ trước) * 100
                        percentageChange = Math.Round(((double)(count - prevTierCount) / prevTierCount) * 100, 2);
                    }
                    else if (count > 0)
                    {
                        percentageChange = 100; // Tăng trưởng 100% nếu kỳ trước bằng 0 và kỳ này có khách
                    }
                }

                periodReport.Tiers.Add(new TierCountResponse
                {
                    TierId = tier.Id,
                    TierName = tier.TierName,
                    Count = count,
                    Percentage = percentage,
                    PercentageChangeFromPrevious = percentageChange
                });
            }

            report.Add(periodReport);
            previousPeriodReport = periodReport; // Lưu lại để so sánh cho chu kỳ tiếp theo
        }

        return report;
    }

    private record BookingDatePrice
    {
        public DateOnly BookingDate { get; init; }
        public decimal TotalPrice { get; init; }
    }

    private static List<RevenueDataPoint> GroupByPeriod(List<BookingDatePrice> bookings, string groupBy)
    {
        Func<BookingDatePrice, string> keySelector = groupBy.ToLower() switch
        {
            "week" => b =>
            {
                var d = b.BookingDate;
                var offset = ((int)d.DayOfWeek + 6) % 7; // Monday = 0
                return d.AddDays(-offset).ToString("yyyy-MM-dd");
            }
            ,
            "month" => b => b.BookingDate.ToString("yyyy-MM"),
            _ => b => b.BookingDate.ToString("yyyy-MM-dd")
        };

        return bookings
            .GroupBy(keySelector)
            .Select(g => new RevenueDataPoint
            {
                Date = g.Key,
                Revenue = g.Sum(x => x.TotalPrice),
                BookingCount = g.Count()
            })
            .OrderBy(d => d.Date)
            .ToList();
    }
}
