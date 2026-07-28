namespace Application.DTOs.Staff;

public class UpdateStaffRequest
{
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public Guid BranchId { get; set; }
    public bool IsAvailable { get; set; }
}
