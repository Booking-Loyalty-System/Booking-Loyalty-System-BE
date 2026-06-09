namespace Domain.Entities;

public class CustomerPromotion
{
    public Guid Id { get; set; }
    public bool IsUsed { get; set; }
    public DateTime UsedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid PromotionId { get; set; }
    
    public Customer Customer { get; set; }
    public Promotion Promotion { get; set; }
}