namespace Application.DTOs.Statistics;

public class TopCustomerResponse
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public decimal TotalSpent { get; set; }
    public int TotalWashes { get; set; }
    public string Tier { get; set; } = null!;
}
