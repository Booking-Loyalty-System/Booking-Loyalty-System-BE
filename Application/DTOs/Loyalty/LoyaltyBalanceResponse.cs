namespace Application.DTOs.Loyalty;

public class LoyaltyBalanceResponse
{
    /// <summary>
    /// [Giữ để tương thích FE cũ] Chứa điểm KHẢ DỤNG (spendable) — đặt tên gây hiểu nhầm.
    /// FE nên chuyển sang đọc <see cref="AvailablePoints"/>.
    /// </summary>
    public int TotalPoints { get; set; }

    /// <summary>Điểm hiện còn dùng được (spendable). Đây là số bị trừ khi đổi voucher.</summary>
    public int AvailablePoints { get; set; }

    /// <summary>Điểm lũy kế trọn đời, dùng để xét hạng (không bị trừ khi đổi voucher).</summary>
    public int LifetimePoints { get; set; }
    public int TotalWashes { get; set; }

    /// <summary>Số lần rửa đã tích trong chu kỳ hiện tại (0..6). Đủ 7 → tặng voucher rửa free. FE hiển thị "x/7".</summary>
    public int CurrentCycleWashes { get; set; }

    public decimal TotalSpent { get; set; }
    public string Tier { get; set; } = null!;
}
