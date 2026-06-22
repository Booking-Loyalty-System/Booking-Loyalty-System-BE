using Domain.Entities;

public class BranchTimeSlot
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public Guid TimeSlotId { get; set; }
    
    // Điểm ăn tiền ở đây: Cấu hình sức chứa tối đa cho khung giờ này.
    // Ví dụ: Chi nhánh A lúc 8h sáng có 5 bệ rửa => MaxCapacity = 5.
    public int MaxCapacity { get; set; } 

    // Có thể thêm cờ để đóng/mở khung giờ này cho chi nhánh
    public bool IsActive { get; set; } = true;

    // Navigations
    public Branch Branch { get; set; } = null!;
    public TimeSlot TimeSlot { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}