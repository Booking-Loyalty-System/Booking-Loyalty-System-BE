namespace Domain.Entities;

public class Staff
{
    public Guid Id { get; set; }
    
    // Nối 1-1 với User để lấy thông tin đăng nhập
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Nối Many-to-1 với Branch (Mỗi staff thuộc 1 chi nhánh)
    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsAvailable { get; set; } = true; // Trạng thái có đang rảnh để làm việc không

    // Nối 1-to-Many với Booking (1 staff làm nhiều booking)
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}