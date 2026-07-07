using Application.DTOs.AdminAnalytic;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AdminAnalyticService : IAdminAnalyticService
    {
        private readonly IApplicationDbContext _context;

        public AdminAnalyticService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardAnalyticResponse> GetRevenueAnalyticsAsync(DashboardFilterRequest filter)
        {
            int currentYear = filter.Year;
            int previousYear = currentYear - 1;

            string filterType = filter.Type.ToUpper();

            var allBranches = await _context.Branches.ToListAsync();

            var query = _context.Bookings
                .Include(b => b.BranchTimeSlot)
                .Where(b => b.Status == BookingStatus.CheckedOut);

            List<Booking> currentPeriodBookings = new();
            List<Booking> previousPeriodBookings = new();

            var chartDataList = new List<RevenueChartResponse>();

            if (filterType == "MONTH")
            {
                int targetMonth = filter.Value ?? DateTime.UtcNow.Month;

                var allBookings = await query
                    .Where(b => b.BookingDate.Month == targetMonth &&
                               (b.BookingDate.Year == currentYear || b.BookingDate.Year == previousYear))
                    .ToListAsync();

                currentPeriodBookings = allBookings.Where(b => b.BookingDate.Year == currentYear).ToList();
                previousPeriodBookings = allBookings.Where(b => b.BookingDate.Year == previousYear).ToList();

                int daysInMonth = DateTime.DaysInMonth(currentYear, targetMonth);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var currentDayData = currentPeriodBookings.Where(b => b.BookingDate.Day == day).ToList();
                    var previousDayData = previousPeriodBookings.Where(b => b.BookingDate.Day == day).ToList();

                    chartDataList.Add(CalculatePeriodData($"Ngày {day:D2}", currentDayData, previousDayData, allBranches));
                }
            }
            else if (filterType == "QUARTER")
            {
                int targetQuarter = filter.Value ?? ((DateTime.UtcNow.Month - 1) / 3 + 1);
                int startMonth = (targetQuarter - 1) * 3 + 1;
                int endMonth = startMonth + 2;

                var allBookings = await query
                    .Where(b => b.BookingDate.Month >= startMonth && b.BookingDate.Month <= endMonth &&
                               (b.BookingDate.Year == currentYear || b.BookingDate.Year == previousYear))
                    .ToListAsync();

                currentPeriodBookings = allBookings.Where(b => b.BookingDate.Year == currentYear).ToList();
                previousPeriodBookings = allBookings.Where(b => b.BookingDate.Year == previousYear).ToList();

                for (int month = startMonth; month <= endMonth; month++)
                {
                    var currentMonthData = currentPeriodBookings.Where(b => b.BookingDate.Month == month).ToList();
                    var previousMonthData = previousPeriodBookings.Where(b => b.BookingDate.Month == month).ToList();

                    chartDataList.Add(CalculatePeriodData($"Tháng {month:D2}", currentMonthData, previousMonthData, allBranches));
                }
            }
            else
            {
                var allBookings = await query
                    .Where(b => b.BookingDate.Year == currentYear || b.BookingDate.Year == previousYear)
                    .ToListAsync();

                currentPeriodBookings = allBookings.Where(b => b.BookingDate.Year == currentYear).ToList();
                previousPeriodBookings = allBookings.Where(b => b.BookingDate.Year == previousYear).ToList();

                // Hiển thị biểu đồ theo 12 THÁNG TRONG NĂM
                for (int month = 1; month <= 12; month++)
                {
                    var currentMonthData = currentPeriodBookings.Where(b => b.BookingDate.Month == month).ToList();
                    var previousMonthData = previousPeriodBookings.Where(b => b.BookingDate.Month == month).ToList();

                    chartDataList.Add(CalculatePeriodData($"Tháng {month:D2}", currentMonthData, previousMonthData, allBranches));
                }
            }

            decimal totalRevenue = currentPeriodBookings.Sum(b => b.TotalPrice);

            var response = new DashboardAnalyticResponse
            {
                TotalRevenue = totalRevenue
            };

            if (filterType == "MONTH")
            {
                response.MonthlyRevenue = chartDataList;
            }
            else if (filterType == "QUARTER")
            {
                response.QuarterlyRevenue = chartDataList;
            }
            else
            {
                response.YearlyRevenue = chartDataList;
            }

            return response;
        }

        private RevenueChartResponse CalculatePeriodData(string label, List<Booking> currentPeriod, List<Booking> previousPeriod, List<Branch> branches)
        {
            decimal currentRev = currentPeriod.Sum(b => b.TotalPrice);
            decimal previousRev = previousPeriod.Sum(b => b.TotalPrice);

            decimal difference = currentRev - previousRev;
            double growthPercentage = 0;

            if (previousRev > 0)
            {
                growthPercentage = (double)(difference / previousRev) * 100;
            }
            else if (currentRev > 0)
            {
                growthPercentage = 100;
            }

            var branchRevenues = branches.Select(b => new BranchRevenueResponse
            {
                BranchId = b.Id,
                BranchName = b.BranchName ?? "Chi nhánh chưa đặt tên",
                Revenue = currentPeriod.Where(x => x.BranchTimeSlot.BranchId == b.Id).Sum(x => x.TotalPrice)
            }).ToList();

            return new RevenueChartResponse
            {
                Label = label,
                CurrentPeriodRevenue = currentRev,
                PreviousPeriodRevenue = previousRev,
                DifferenceAmount = difference,
                GrowthPercentage = Math.Round(growthPercentage, 2),
                BranchRevenues = branchRevenues
            };
        }
    }
}
