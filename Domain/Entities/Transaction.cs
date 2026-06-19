namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public string TransactionCode { get; set; }
    public bool Status { get; set; }
    
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; }
}