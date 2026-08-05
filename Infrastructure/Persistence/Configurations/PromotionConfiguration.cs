using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.DiscountType)
            .IsRequired();

        builder.Property(p => p.DiscountValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.MinSpend)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(p => p.MaxDiscount)
            .HasPrecision(18, 2)
            .IsRequired(false);

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

        builder.HasMany(p => p.PromotionBranches)
            .WithOne(pb => pb.Promotion)
            .HasForeignKey(pb => pb.PromotionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.PriorityLevel)
            .IsRequired();

        builder.HasData(
            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"),
                Code = "TB-PERCENT",
                Name = "Ưu đãi Tân Bình",
                Description = "Giảm 10% cho toàn bộ hóa đơn tại Tân Bình",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10.00m,
                PriorityLevel = 1,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = 100000,
                MaxDiscount = 50000m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000002"),
                Code = "Q3-PERCENT",
                Name = "Ưu đãi Quận 3",
                Description = "Giảm 15% cho toàn bộ hóa đơn tại Quận 3",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 15.00m,
                PriorityLevel = 1,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = 150000,
                MaxDiscount = 100000m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000003"),
                Code = "Q9-PERCENT",
                Name = "Ưu đãi Quận 9",
                Description = "Giảm 20% cho toàn bộ hóa đơn tại Quận 9",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 20.00m,
                PriorityLevel = 1,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = 200000,
                MaxDiscount = 150000m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000004"),
                Code = "BRONZE-5",
                Name = "Ưu đãi hạng Bronze",
                Description = "Giảm 5% cho thành viên Đồng",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 5.00m,
                PriorityLevel = 2,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = null,
                MaxDiscount = 20000m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000005"),
                Code = "SILVER-10",
                Name = "Ưu đãi hạng Silver",
                Description = "Giảm 10% cho thành viên Bạc",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10.00m,
                PriorityLevel = 3,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = 150000,
                MaxDiscount = 50000m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000006"),
                Code = "GOLD-15",
                Name = "Đặc quyền hạng Gold",
                Description = "Giảm 15% cho thành viên Vàng",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 15.00m,
                PriorityLevel = 4,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = null,
                MaxDiscount = 150000m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000007"),
                Code = "DIAMOND-VIP",
                Name = "Đẳng cấp Diamond",
                Description = "Giảm 25% tối đa đặc quyền Kim Cương",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 25.00m,
                PriorityLevel = 5,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = null,
                MaxDiscount = 300000m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000008"),
                Code = "BDAY-15",
                Name = "Mừng Sinh Nhật 15%",
                Description = "Giảm 15% trong ngày sinh nhật của bạn",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 15.00m,
                PriorityLevel = 10,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = 200000,
                MaxDiscount = 200000m,
                IsActive = true,
                RequiresBirthday = true,
                CreatedAt = DateTime.UtcNow
            },
            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000009"),
                Code = "BDAY-HAPPY",
                Name = "Sinh Nhật Vui Vẻ 5%",
                Description = "Giảm 5% cho hóa đơn đặt trước vào tuần sinh nhật",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 5.00m,
                PriorityLevel = 10,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = 500000,
                MaxDiscount = 100000m,
                IsActive = true,
                RequiresBirthday = true,
                CreatedAt = DateTime.UtcNow
            },
            new Promotion
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000010"),
                Code = "BDAY-MEGA",
                Name = "Đại Tiệc Sinh Nhật 20%",
                Description = "Giảm tối đa 20% cho hóa đơn đặt tiệc sinh nhật lớn",
                DiscountType = DiscountType.Percentage,
                DiscountValue = 20.00m,
                PriorityLevel = 9,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                MaxUses = 1,
                MinSpend = 1000000,
                MaxDiscount = 500000m,
                IsActive = true,
                RequiresBirthday = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}