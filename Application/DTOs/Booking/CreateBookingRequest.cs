namespace Application.DTOs.Booking;

public class CreateBookingRequest
{
    public Guid BranchId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid WashPackageId { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly StartTime { get; set; }

    /// <summary>Optional promotion code to discount the booking price.</summary>
    public string? PromotionCode { get; set; }

    /// <summary>Optional add-on services to attach to the booking.</summary>
    public List<Guid>? AddOnIds { get; set; }

    /// <summary>Optional redeemed-voucher (RewardRedemption) ID to apply a fixed-amount discount.</summary>
    public Guid? RewardRedemptionId { get; set; }
}
