namespace Application.DTOs.WashBay;

public class WashBayResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Status { get; set; } = null!;
    public List<string> SupportedTypes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public Guid? BranchId { get; set; }
}
