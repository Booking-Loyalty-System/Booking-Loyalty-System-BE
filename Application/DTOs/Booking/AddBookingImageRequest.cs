namespace Application.DTOs.Booking;

public class AddBookingImageRequest
{
    /// <summary>URL ảnh đã upload lên Firebase Storage (FE gửi về).</summary>
    public string ImageUrl { get; set; } = null!;

    /// <summary>"BeforeWash" hoặc "AfterWash".</summary>
    public string Type { get; set; } = null!;

    public string? Note { get; set; }
}
