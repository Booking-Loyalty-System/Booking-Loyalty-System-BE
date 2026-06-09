namespace Domain.Entities;

public class PointHistory
{
    public Guid Id { get; set; }    
    public string Description { get; set; } 
    public DateOnly ExpiryDate { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? RewardId { get; set; }
    
    public Customer Customer { get; set; }
    public Reward? Reward { get; set; }
}