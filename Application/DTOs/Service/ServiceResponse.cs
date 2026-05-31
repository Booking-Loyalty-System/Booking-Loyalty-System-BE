namespace Application.DTOs.Service;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public int DurationMinutes { get; set; }
    public List<string> Features { get; set; } = new();
}
