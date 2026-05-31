namespace Application.DTOs.WashBay;

public class CreateWashBayRequest
{
    public string Name { get; set; } = null!;
    public List<string> SupportedTypes { get; set; } = new();
}
