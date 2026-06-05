using Domain.Enums;

namespace Domain.Entities;

public class Branch
{
    public Guid Id { get; set; }
    public string BranchName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Hotline { get; set; } = null!;
    public string OperatingHours { get; set; } = null!;
    public BranchStatus Status { get; set; }
    public ICollection<WashBay> WashBays { get; set; } = new List<WashBay>();
}