using Application.DTOs.Report;
using Application.Interfaces;
using ClosedXML.Excel;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IApplicationDbContext _context;

        public ReportService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RevenueReportResponse> GetRevenueReportAsync(RevenueReportRequest request)
        {
            // Chuyển DateTime sang DateOnly để query chuẩn với BookingDate
            var currentFromDate = DateOnly.FromDateTime(request.CurrentFromDate);
            var currentToDate = DateOnly.FromDateTime(request.CurrentToDate);

            // Tính doanh thu kỳ hiện tại từ Booking
            var currentRevenue = await _context.Bookings
                .Where(b => b.BookingDate >= currentFromDate
                         && b.BookingDate <= currentToDate
                         && b.Status == BookingStatus.CheckedOut) // Chỉ tính booking đã hoàn thành
                .SumAsync(b => (decimal?)b.TotalPrice) ?? 0m;

            decimal compareRevenue = 0m;

            // Tính doanh thu kỳ so sánh (nếu có)
            if (request.CompareFromDate.HasValue && request.CompareToDate.HasValue)
            {
                var compareFromDate = DateOnly.FromDateTime(request.CompareFromDate.Value);
                var compareToDate = DateOnly.FromDateTime(request.CompareToDate.Value);

                compareRevenue = await _context.Bookings
                    .Where(b => b.BookingDate >= compareFromDate
                             && b.BookingDate <= compareToDate
                             && b.Status == BookingStatus.CheckedOut)
                    .SumAsync(b => (decimal?)b.TotalPrice) ?? 0m;
            }

            return new RevenueReportResponse
            {
                CurrentFromDate = request.CurrentFromDate,
                CurrentToDate = request.CurrentToDate,
                CurrentRevenue = currentRevenue,
                CompareFromDate = request.CompareFromDate,
                CompareToDate = request.CompareToDate,
                CompareRevenue = compareRevenue
            };
        }

        public async Task<byte[]> ExportRevenueToExcelAsync(RevenueReportRequest request)
        {
            var data = await GetRevenueReportAsync(request);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Revenue Report");

            // Style Tiêu đề
            worksheet.Cell("A1").Value = "BÁO CÁO DOANH THU & SO SÁNH";
            worksheet.Range("A1:E1").Merge().Style
                .Font.SetBold(true)
                .Font.SetFontSize(16)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // --- XỬ LÝ TEXT THỜI GIAN GIỐNG UI (Month X Year Y) ---
            bool isMonthCurrent = data.CurrentFromDate.Day == 1 && data.CurrentToDate.Day == DateTime.DaysInMonth(data.CurrentToDate.Year, data.CurrentToDate.Month);
            worksheet.Cell("A3").Value = "Kỳ báo cáo:";
            worksheet.Cell("A3").Style.Font.Bold = true;
            worksheet.Cell("B3").Value = isMonthCurrent
                ? $"Month {data.CurrentFromDate.Month} Year {data.CurrentFromDate.Year}"
                : $"{data.CurrentFromDate:dd/MM/yyyy} - {data.CurrentToDate:dd/MM/yyyy}";

            if (data.CompareFromDate.HasValue && data.CompareToDate.HasValue)
            {
                bool isMonthCompare = data.CompareFromDate.Value.Day == 1 && data.CompareToDate.Value.Day == DateTime.DaysInMonth(data.CompareToDate.Value.Year, data.CompareToDate.Value.Month);
                worksheet.Cell("A4").Value = "Kỳ so sánh:";
                worksheet.Cell("A4").Style.Font.Bold = true;
                worksheet.Cell("B4").Value = isMonthCompare
                    ? $"Month {data.CompareFromDate.Value.Month} Year {data.CompareFromDate.Value.Year}"
                    : $"{data.CompareFromDate:dd/MM/yyyy} - {data.CompareToDate:dd/MM/yyyy}";
            }

            // Header bảng
            worksheet.Cell("A6").Value = "Hạng mục";
            worksheet.Cell("B6").Value = "Doanh thu kỳ hiện tại";
            worksheet.Cell("C6").Value = "Doanh thu kỳ so sánh";
            worksheet.Cell("D6").Value = "Chênh lệch (Variance)";
            worksheet.Cell("E6").Value = "Tăng trưởng (%)";

            var headerRange = worksheet.Range("A6:E6");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E293B"); // Nền xám đen giống UI
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Dữ liệu
            worksheet.Cell("A7").Value = "Tổng doanh thu";
            worksheet.Cell("B7").Value = data.CurrentRevenue;
            worksheet.Cell("C7").Value = data.CompareRevenue;
            worksheet.Cell("D7").Value = data.Variance;
            worksheet.Cell("E7").Value = data.GrowthPercentage / 100;

            // --- ĐỊNH DẠNG SỐ CÓ MÀU SẮC GIỐNG UI ---

            // 1. Tiền tệ bình thường cho 2 cột doanh thu
            worksheet.Range("B7:C7").Style.NumberFormat.Format = "#,##0 \"đ\"";

            // 2. Định dạng cột Variance: Số dương màu xanh có chữ Growth, Số âm màu đỏ có chữ Decline
            worksheet.Cell("D7").Style.NumberFormat.Format = "[Green]\"Growth: \"+#,##0 \"đ\";[Red]\"Decline: \"-#,##0 \"đ\";\"Variance: 0 đ\"";

            // 3. Định dạng cột %: Dương có dấu cộng màu xanh, Âm màu đỏ
            worksheet.Cell("E7").Style.NumberFormat.Format = "[Green]+0.00%;[Red]-0.00%;0.00%";

            // Bôi đậm cột Variance và Tăng trưởng để nổi bật
            worksheet.Range("D7:E7").Style.Font.SetBold(true);

            // Tự động căn chỉnh độ rộng cột
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}