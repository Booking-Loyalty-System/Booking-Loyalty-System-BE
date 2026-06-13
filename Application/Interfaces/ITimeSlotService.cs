using Application.DTOs.TimeSlot;

namespace Application.Interfaces;

public interface ITimeSlotService
{
    Task<List<DailyTimeSlotsSummaryResponse>> GetWeeklySlotsSummaryAsync(Guid branchId, DateOnly startDate);
}