using Application.DTOs.AdminAnalytic;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAnalyticController : ControllerBase
    {
        private readonly IAdminAnalyticService _service;

        public AdminAnalyticController(IAdminAnalyticService service)
        {
            _service = service;
        }

        [HttpGet("revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRevenueAnalytics([FromQuery] DashboardFilterRequest filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Type))
            {
                filter.Type = "YEAR";
            }

            if (filter.Year == 0)
            {
                filter.Year = DateTime.UtcNow.Year;
            }

            var result = await _service.GetRevenueAnalyticsAsync(filter);
            return Ok(result);
        }
    }
}
