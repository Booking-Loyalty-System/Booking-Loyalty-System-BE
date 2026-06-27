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
    public decimal TotalSpent { get; set; }
    public string Tier { get; set; } = null!;
}
