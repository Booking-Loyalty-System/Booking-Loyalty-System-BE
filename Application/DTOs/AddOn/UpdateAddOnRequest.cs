namespace Application.DTOs.AddOn;

public class UpdateAddOnRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? DurationMinutes { get; set; }
    public bool? IsActive { get; set; }
}
