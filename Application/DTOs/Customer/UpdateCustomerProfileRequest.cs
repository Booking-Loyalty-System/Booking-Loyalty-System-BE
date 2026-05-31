namespace Application.DTOs.Customer;

public class UpdateCustomerProfileRequest
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
