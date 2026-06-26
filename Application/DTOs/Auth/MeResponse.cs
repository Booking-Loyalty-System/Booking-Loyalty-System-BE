namespace Application.DTOs.Auth;

public class MeResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Tier { get; set; }

    /// <summary>Điểm hiện còn dùng được (spendable). Số bị trừ khi đổi voucher.</summary>
    public int? AvailablePoints { get; set; }
    public int? TotalWashes { get; set; }
}
