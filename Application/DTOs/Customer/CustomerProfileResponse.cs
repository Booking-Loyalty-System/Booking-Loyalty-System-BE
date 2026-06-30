namespace Application.DTOs.Customer;

public class CustomerProfileResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Tier { get; set; } = null!;
    public int AvailablePoint { get; set; }
    public int TotalWashes { get; set; }

    /// <summary>Số lần rửa đã tích trong chu kỳ hiện tại (0..6). Đủ 7 → tặng voucher rửa free. FE hiển thị "x/7".</summary>
    public int CurrentCycleWashes { get; set; }

    public int TotalPoint { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime CreatedAt { get; set; }
}
