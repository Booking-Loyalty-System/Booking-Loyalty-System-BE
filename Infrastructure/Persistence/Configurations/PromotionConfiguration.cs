using Domain.Entities;
using Domain.Enums;
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

        builder.Property(p => p.RequiresBirthday)
            .HasDefaultValue(false)
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
        
        // Lưu ý: DiscountType giả định bạn có Enum: 0 = FixedAmount, 1 = Percentage
builder.HasData(
    // === NHÓM 1: DÀNH RIÊNG CHO CHI NHÁNH ===
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"), Code = "TB-PERCENT", Name = "Ưu đãi Tân Bình", Description = "Giảm 10% cho toàn bộ hóa đơn tại Tân Bình", DiscountType = DiscountType.Percentage, DiscountValue = 10.00m, PriorityLevel = 1, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = 500, MinSpend = 100000, IsActive = true, CreatedAt = DateTime.UtcNow 
    },
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000002"), Code = "Q3-PERCENT", Name = "Ưu đãi Quận 3", Description = "Giảm 15% cho toàn bộ hóa đơn tại Quận 3", DiscountType = DiscountType.Percentage, DiscountValue = 15.00m, PriorityLevel = 1, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = 500, MinSpend = 150000, IsActive = true, CreatedAt = DateTime.UtcNow 
    },
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000003"), Code = "Q9-PERCENT", Name = "Ưu đãi Quận 9", Description = "Giảm 20% cho toàn bộ hóa đơn tại Quận 9", DiscountType = DiscountType.Percentage, DiscountValue = 20.00m, PriorityLevel = 1, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = 500, MinSpend = 200000, IsActive = true, CreatedAt = DateTime.UtcNow 
    },

    // === NHÓM 2: DÀNH RIÊNG CHO HẠNG THÀNH VIÊN ===
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000004"), Code = "BRONZE-10K", Name = "Ưu đãi hạng Bronze", Description = "Giảm 5% cho thành viên Đồng", DiscountType = DiscountType.FixedAmount, DiscountValue = 5.00m, PriorityLevel = 2, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = null, MinSpend = null, IsActive = true, CreatedAt = DateTime.UtcNow 
    },
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000005"), Code = "SILVER-50K", Name = "Ưu đãi hạng Silver", Description = "Giảm 10% cho thành viên Bạc", DiscountType = DiscountType.FixedAmount, DiscountValue = 10.00m, PriorityLevel = 3, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = null, MinSpend = 150000, IsActive = true, CreatedAt = DateTime.UtcNow 
    },
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000006"), Code = "GOLD-15", Name = "Đặc quyền hạng Gold", Description = "Giảm 15% cho thành viên Vàng", DiscountType = DiscountType.Percentage, DiscountValue = 15.00m, PriorityLevel = 4, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = null, MinSpend = null, IsActive = true, CreatedAt = DateTime.UtcNow 
    },
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000007"), Code = "DIAMOND-VIP", Name = "Đẳng cấp Diamond", Description = "Giảm 25% tối đa đặc quyền Kim Cương", DiscountType = DiscountType.Percentage, DiscountValue = 25.00m, PriorityLevel = 5, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = null, MinSpend = null, IsActive = true, CreatedAt = DateTime.UtcNow 
    },

    // === NHÓM 3: KHUYẾN MÃI SINH NHẬT (Không liên kết Branch, không liên kết Tier) ===
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000008"), Code = "BDAY-15", Name = "Mừng Sinh Nhật 15%", Description = "Giảm 15% trong ngày sinh nhật của bạn", DiscountType = DiscountType.Percentage, DiscountValue = 15.00m, PriorityLevel = 10, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = null, MinSpend = 200000, IsActive = true, RequiresBirthday = true, CreatedAt = DateTime.UtcNow
    },
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000009"), Code = "BDAY-HAPPY", Name = "Sinh Nhật Vui Vẻ 5%", Description = "Giảm 5% cho hóa đơn đặt trước vào tuần sinh nhật", DiscountType = DiscountType.Percentage, DiscountValue = 5.00m, PriorityLevel = 10, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = null, MinSpend = 500000, IsActive = true, RequiresBirthday = true, CreatedAt = DateTime.UtcNow
    },
    new Promotion { 
        Id = Guid.Parse("c0000000-0000-0000-0000-000000000010"), Code = "BDAY-MEGA", Name = "Đại Tiệc Sinh Nhật 20%", Description = "Giảm tối đa 20% cho hóa đơn đặt tiệc sinh nhật lớn", DiscountType = DiscountType.Percentage, DiscountValue = 20.00m, PriorityLevel = 9, StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), MaxUses = null, MinSpend = 1000000, IsActive = true, RequiresBirthday = true, CreatedAt = DateTime.UtcNow
    }
);
    }
}