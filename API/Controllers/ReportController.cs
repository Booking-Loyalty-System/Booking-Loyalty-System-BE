using Application.DTOs.Report;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("revenue/export-excel")]
        public async Task<IActionResult> ExportExcel([FromQuery] RevenueReportRequest request)
        {
            var fileBytes = await _reportService.ExportRevenueToExcelAsync(request);
            var fileName = $"Revenue_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
