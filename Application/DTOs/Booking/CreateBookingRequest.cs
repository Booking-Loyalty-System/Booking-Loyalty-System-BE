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

    /// <summary>Optional voucher (CustomerPromotion) ID to apply a personal voucher discount.</summary>
    public Guid? VoucherId { get; set; }
}
