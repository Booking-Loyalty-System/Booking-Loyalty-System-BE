namespace Domain.Entities;

public class Promotion
{
    public Guid Id { get; set; }    
    public string Name { get; set; }
    public decimal Discount { get; set; }
    public int PriorityLevel { get; set; }

    public ICollection<PromotionBranch> PromotionBranches = new List<PromotionBranch>();
}