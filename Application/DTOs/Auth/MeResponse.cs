namespace Application.DTOs.Auth;

public class MeResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Tier { get; set; }
    public int? TotalPoints { get; set; }
    public int? AvailablePoints { get; set; }
    public int? TotalWashes { get; set; }
}
