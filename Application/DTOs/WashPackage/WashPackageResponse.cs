namespace Application.DTOs.WashPackage;

public class WashPackageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public List<string> Features { get; set; } = new();
    public string? VehicleType { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
