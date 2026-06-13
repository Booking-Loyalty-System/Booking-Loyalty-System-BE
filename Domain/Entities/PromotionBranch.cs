namespace Domain.Entities;

public class PromotionBranch
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public Guid PromotionId { get; set; }
    public Guid BranchId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public Promotion Promotion { get; set; }
    public Branch Branch { get; set; }
}