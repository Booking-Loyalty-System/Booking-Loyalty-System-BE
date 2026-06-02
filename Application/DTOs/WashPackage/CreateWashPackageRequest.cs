namespace Application.DTOs.WashPackage;

public class CreateWashPackageRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public List<string>? Features { get; set; }
    public string? VehicleType { get; set; }
}
