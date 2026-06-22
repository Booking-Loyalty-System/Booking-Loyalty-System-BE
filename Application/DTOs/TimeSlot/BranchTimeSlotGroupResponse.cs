namespace Application.DTOs.TimeSlot;

public class BranchTimeSlotGroupResponse
{
    public Guid TimeSlotId { get; set; }
    public TimeOnly StartTime { get; set; }

    public string SlotRatio { get; set; } = string.Empty; 
    public bool IsAvailable { get; set; }
}

public class DailyTimeSlotsSummaryResponse
{
    public DateOnly Date { get; set; } // Ngày (VD: 2026-06-13)
    public string DayOfWeek { get; set; } = string.Empty; // Tên thứ (VD: Saturday)
    
    // Danh sách 11 ca giờ đổ ra vẽ UI mẫu
    public List<BranchTimeSlotGroupResponse> TimeSlots { get; set; } = new();
}