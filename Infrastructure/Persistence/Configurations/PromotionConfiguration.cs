using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        // Khóa chính
        builder.HasKey(p => p.Id);

        // Cấu hình mã giảm giá (Code) - Bắt buộc, tối đa 50 ký tự
        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

        // Tạo Index cho Code để đảm bảo là Unique (không trùng lặp) và tìm kiếm nhanh hơn
        builder.HasIndex(p => p.Code)
            .IsUnique();

        // Tên chương trình khuyến mãi
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        // Mô tả có thể null, độ dài tối đa 1000 ký tự
        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        // Enum lưu dưới dạng String hoặc Int trong DB (mặc định EF Core lưu dạng Int)
        builder.Property(p => p.DiscountType)
            .IsRequired();

        // Giá trị giảm giá (Ví dụ: 10.00% hoặc 50000.00 VND)
        builder.Property(p => p.DiscountValue)
            .HasPrecision(18, 2) // Dùng 18,2 chuẩn cho tiền tệ và số lớn thay vì (5,2) cũ
            .IsRequired();

        // Chi tiêu tối thiểu (Có thể null)
        builder.Property(p => p.MinSpend)
            .HasPrecision(18, 2)
            .IsRequired(false);

        // Cấu hình các trường số lượng sử dụng và ngày tháng
        builder.Property(p => p.MaxUses)
            .IsRequired(false);

        builder.Property(p => p.UsedCount)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(p => p.StartDate)
            .IsRequired();

        builder.Property(p => p.EndDate)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // Giữ lại mối quan hệ cũ với PromotionBranch của bạn
        builder.HasMany(p => p.PromotionBranches)
            .WithOne(pb => pb.Promotion)
            .HasForeignKey(pb => pb.PromotionId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Property(p => p.PriorityLevel)
            .IsRequired();

        builder.Property(p => p.IsVoucher)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(p => p.PointsCost)
            .IsRequired(false);
    }
}