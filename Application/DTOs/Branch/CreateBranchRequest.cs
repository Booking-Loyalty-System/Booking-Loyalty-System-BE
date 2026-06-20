namespace Application.DTOs.Branch;

public class CreateBranchRequest
{
    public string BranchName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Hotline { get; set; } = null!;
    public string OperatingHours { get; set; } = null!;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
