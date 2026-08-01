namespace Application.DTOs.Staff;

public class CreateStaffRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public Guid BranchId { get; set; }
}