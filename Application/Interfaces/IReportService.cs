using Application.DTOs.Report;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task<RevenueReportResponse> GetRevenueReportAsync(RevenueReportRequest request);
        Task<byte[]> ExportRevenueToExcelAsync(RevenueReportRequest request);
    }
}
