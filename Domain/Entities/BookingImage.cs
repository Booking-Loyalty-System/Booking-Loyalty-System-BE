using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Ảnh xe của một booking (trước/sau khi rửa). File nhị phân do FE upload thẳng lên
/// Firebase Storage; BE chỉ lưu URL tải về. Một booking có nhiều ảnh (nhiều góc chụp).
/// </summary>
public class BookingImage
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }

    /// <summary>URL tải ảnh (Firebase Storage download URL).</summary>
    public string ImageUrl { get; set; } = null!;

    public BookingImageType Type { get; set; }

    public string? Note { get; set; }

    /// <summary>Nhân viên đã gắn ảnh (null nếu không xác định được).</summary>
    public Guid? UploadedByStaffId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Booking Booking { get; set; } = null!;
    public Staff? UploadedByStaff { get; set; }
}
