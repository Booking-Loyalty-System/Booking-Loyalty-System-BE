namespace Application.DTOs.Staff;

public class StaffProfileResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsAvailable { get; set; }
    public string Role { get; set; } = null!;

    // Thông tin chi nhánh đính kèm
    public StaffBranchResponse Branch { get; set; } = null!;
}

public class StaffBranchResponse
{
    public Guid Id { get; set; }
    public string BranchName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Hotline { get; set; } = null!;
}