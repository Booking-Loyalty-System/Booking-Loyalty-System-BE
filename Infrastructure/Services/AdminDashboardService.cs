using System.Text;
using Application.DTOs.AdminDashboard;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AdminDashboardService : IAdminDashboardService
{
    private static TierConfigDto _tierConfig = new()
    {
        MemberMultiplier = 1,
        SilverMultiplier = 1.5,
        GoldMultiplier = 2,
        PlatinumMultiplier = 3
    };

    private readonly IApplicationDbContext _context;

    public AdminDashboardService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryResponse> GetDashboardSummaryAsync()
    {
        var now = DateTime.UtcNow;
        // Fix lỗi PostgreSQL: Thêm tham số DateTimeKind.Utc
        var currentMonthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var monthlyBookings = await _context.Bookings
            .Where(b => b.CreatedAt >= currentMonthStart)
            .ToListAsync();

        var completedBookings = monthlyBookings.Where(b => b.Status == BookingStatus.Completed).ToList();
        var totalRevenue = completedBookings.Sum(b => b.TotalPrice);
        var totalBookings = monthlyBookings.Count;
        var activeCustomers = monthlyBookings.Select(b => b.CustomerId).Distinct().Count();
        var aov = totalBookings > 0 ? totalRevenue / totalBookings : 0;

        var revenueChart = new List<RevenueDataDto>();
        for (int i = 4; i >= 0; i--)
        {
            var targetMonth = now.AddMonths(-i);
            // Fix lỗi PostgreSQL: Thêm tham số DateTimeKind.Utc
            var startDate = new DateTime(targetMonth.Year, targetMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = startDate.AddMonths(1);

            var monthRevenue = await _context.Bookings
                .Where(b => b.Status == BookingStatus.Completed && b.CreatedAt >= startDate && b.CreatedAt < endDate)
                .SumAsync(b => b.TotalPrice);

            revenueChart.Add(new RevenueDataDto
            {
                Month = targetMonth.ToString("MMM"),
                Revenue = monthRevenue
            });
        }

        var tierDistribution = new List<TierDistributionDto>
        {
            new() { Name = "Bronze", Value = await _context.Customers.CountAsync(c => c.Tier.TierName == "Bronze"), Color = "#3b82f6" },
            new() { Name = "Silver", Value = await _context.Customers.CountAsync(c => c.Tier.TierName == "Silver"), Color = "#9ca3af" },
            new() { Name = "Gold", Value = await _context.Customers.CountAsync(c => c.Tier.TierName == "Gold"), Color = "#eab308" },
            new() { Name = "Diamond", Value = await _context.Customers.CountAsync(c => c.Tier.TierName == "Diamond"), Color = "#a855f7" }
        };

        return new DashboardSummaryResponse
        {
            Metrics = new MetricsDto
            {
                TotalRevenue = totalRevenue,
                TotalBookings = totalBookings,
                ActiveCustomers = activeCustomers,
                AverageOrderValue = aov
            },
            RevenueChart = revenueChart,
            TierDistribution = tierDistribution
        };
    }

    public async Task<List<RecentBookingResponse>> GetRecentBookingsAsync(int limit = 5)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.WashPackage)
            .OrderByDescending(b => b.CreatedAt)
            .Take(limit)
            .Select(b => new RecentBookingResponse
            {
                Id = b.Id.ToString(),
                Customer = b.Customer.FullName,
                Service = b.WashPackage.Name,
                Amount = b.TotalPrice,
                Status = b.Status.ToString()
            })
            .ToListAsync();
    }

    public Task<TierConfigDto> GetTierConfigAsync()
    {
        return Task.FromResult(new TierConfigDto
        {
            MemberMultiplier = _tierConfig.MemberMultiplier,
            SilverMultiplier = _tierConfig.SilverMultiplier,
            GoldMultiplier = _tierConfig.GoldMultiplier,
            PlatinumMultiplier = _tierConfig.PlatinumMultiplier
        });
    }

    public Task UpdateTierConfigAsync(TierConfigDto request)
    {
        _tierConfig = new TierConfigDto
        {
            MemberMultiplier = request.MemberMultiplier,
            SilverMultiplier = request.SilverMultiplier,
            GoldMultiplier = request.GoldMultiplier,
            PlatinumMultiplier = request.PlatinumMultiplier
        };

        return Task.CompletedTask;
    }

    public async Task<byte[]> ExportRBLDatasetAsync()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Customer)
                .ThenInclude(c => c.Tier)
            .Include(b => b.WashPackage)
            .OrderByDescending(b => b.CreatedAt)
            .Take(1000)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Timestamp,CustomerID,Tier,BookingID,ServiceType,Amount,PointsEarned,LoyaltyEvent");

        foreach (var b in bookings)
        {
            sb.AppendLine($"{b.CreatedAt:yyyy-MM-dd HH:mm:ss},{b.CustomerId},{b.Customer.Tier.TierName},{b.Id},{b.WashPackage.Name},{b.TotalPrice},0,Booking Created");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public async Task<List<TopCustomerDto>> GetTopCustomersAsync(string timeFrame, int limit = 10)
    {
        var startDate = GetStartDateByTimeFrame(timeFrame);

        return await _context.Bookings
            .Include(b => b.Customer)
                .ThenInclude(c => c.Tier)
            .Include(b => b.Customer)
                .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Point)

            .Where(b => b.Status == BookingStatus.Completed && b.CreatedAt >= startDate)
            .GroupBy(b => new
            {
                b.Customer.Id,
                b.Customer.FullName,
                b.Customer.Tier.TierName,
                AvailablePoints = b.Customer.User.Point != null ? b.Customer.User.Point.AvailablePoints : 0
            })
            .Select(g => new TopCustomerDto
            {
                CustomerId = g.Key.Id.ToString(),
                CustomerName = g.Key.FullName,
                TierName = g.Key.TierName ?? "Member",
                TotalSpent = g.Sum(b => b.TotalPrice),
                LastVisit = g.Max(b => b.CreatedAt),
                Points = g.Key.AvailablePoints // Gán vào DTO
            })
            .OrderByDescending(c => c.TotalSpent)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<TopBranchDto>> GetTopBranchesAsync(string timeFrame)
    {
        var startDate = GetStartDateByTimeFrame(timeFrame);

        return await _context.Bookings
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.Branch)
            .Where(b => b.Status == BookingStatus.Completed && b.CreatedAt >= startDate)
            .GroupBy(b => b.BranchTimeSlot.Branch)
            .Select(g => new TopBranchDto
            {
                BranchId = g.Key.Id.ToString(),
                BranchName = g.Key.BranchName,
                TotalRevenue = g.Sum(b => b.TotalPrice),
                TotalBookings = g.Count()
            })
            .OrderByDescending(b => b.TotalRevenue)
            .ToListAsync();
    }

    public async Task<List<TopTimeSlotDto>> GetTopTimeSlotsAsync(string timeFrame, int limit = 5)
    {
        var startDate = GetStartDateByTimeFrame(timeFrame);

        return await _context.Bookings
            .Include(b => b.BranchTimeSlot)
                .ThenInclude(bts => bts.TimeSlot)
            .Where(b => b.Status == BookingStatus.Completed && b.CreatedAt >= startDate)
            .GroupBy(b => b.BranchTimeSlot.TimeSlot)
            .Select(g => new TopTimeSlotDto
            {
                TimeSlotId = g.Key.Id.ToString(),
                TimeSlotDisplay = $"{g.Key.StartTime}",
                TotalBookings = g.Count()
            })
            .OrderByDescending(t => t.TotalBookings)
            .Take(limit)
            .ToListAsync();
    }

    private DateTime GetStartDateByTimeFrame(string timeFrame)
    {
        var now = DateTime.UtcNow;
        return timeFrame.ToLower() switch
        {
            "month" => new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
            "quarter" => new DateTime(now.Year, ((now.Month - 1) / 3) * 3 + 1, 1, 0, 0, 0, DateTimeKind.Utc),
            "year" => new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            _ => new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc)
        };
    }
}