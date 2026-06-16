namespace Application.DTOs.Branch;

public class UpdateBranchRequest
{
    public string? BranchName { get; set; }
    public string? Address { get; set; }
    public string? Hotline { get; set; }
    public string? OperatingHours { get; set; }
    public string? Status { get; set; }
}
