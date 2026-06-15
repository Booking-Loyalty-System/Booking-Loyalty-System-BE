namespace Application.DTOs.Branch;

public class BranchResponse
{
    public Guid Id { get; set; }
    public string BranchName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Hotline { get; set; } = null!;
    public string OperatingHours { get; set; } = null!;
    public string Status { get; set; } = null!;
    public double Longitude { get; set; } 
    public double Latitude { get; set; }
}
