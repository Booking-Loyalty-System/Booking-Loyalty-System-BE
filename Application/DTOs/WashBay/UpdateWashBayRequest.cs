namespace Application.DTOs.WashBay;

public class UpdateWashBayRequest
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public List<string>? SupportedTypes { get; set; }
}
