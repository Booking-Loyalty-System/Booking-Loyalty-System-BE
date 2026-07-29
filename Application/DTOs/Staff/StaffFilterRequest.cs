namespace Application.DTOs.Staff;

public class StaffFilterRequest
{
    public Guid? BranchId { get; set; }
    public string? Search { get; set; }
    public bool? IsActive { get; set; }
}
