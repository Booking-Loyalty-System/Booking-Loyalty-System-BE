using Domain.Enums;

namespace Domain.Entities;

public class Branch
{
    public Guid Id { get; set; }
    public string BranchName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Hotline { get; set; } = null!;
    public string OperatingHours { get; set; } = null!;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public BranchStatus Status { get; set; }
    public ICollection<Staff> Staffs { get; set; } = new List<Staff>();
    public ICollection<WashBay> WashBays { get; set; } = new List<WashBay>();
    public ICollection<BranchTimeSlot> BranchTimeSlots { get; set; } = new List<BranchTimeSlot>();
    public ICollection<PromotionBranch> PromotionBranches { get; set; } = new List<PromotionBranch>();
}