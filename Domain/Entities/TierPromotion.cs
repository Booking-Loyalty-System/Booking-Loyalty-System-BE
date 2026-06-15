namespace Domain.Entities;

public class TierPromotion
{
    public Guid TierPromotionId { get; set; }
    public Guid TierId { get; set; }
    public Guid PromotionId { get; set; }
    
    public Tier Tier { get; set; }
    public Promotion Promotion { get; set; }
}