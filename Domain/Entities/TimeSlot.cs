public class TimeSlot
{
    public Guid Id { get; set; }
    public TimeOnly StartTime { get; set; }
    
    // Navigation
    public ICollection<BranchTimeSlot> BranchTimeSlots { get; set; } = new List<BranchTimeSlot>();
}