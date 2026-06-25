using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RewardConfiguration : IEntityTypeConfiguration<Reward>
{
    public void Configure(EntityTypeBuilder<Reward> builder)
    {
        builder.ToTable("Rewards");

        // Khóa chính
        builder.HasKey(r => r.Id);

        // Cấu hình các thuộc tính thời gian
        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.EndDate)
            .IsRequired();
        
        builder.Property(r => r.Status)
            .IsRequired()
            .HasDefaultValue(true);

        // Fixed-amount discount this reward/voucher grants when applied to a booking.
        builder.Property(r => r.DiscountAmount)
            .HasPrecision(18, 2);

        // Voucher code shown to the customer.
        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(r => r.Bookings)
            .WithOne(b => b.Reward)
            .HasForeignKey(b => b.RewardId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional wash package for free-wash vouchers.
        builder.HasOne(r => r.WashPackage)
            .WithMany()
            .HasForeignKey(r => r.WashPackageId)
            .OnDelete(DeleteBehavior.SetNull);

      // DiscountAmount luu bang VND DAY DU (khop voi WashPackage.Price: 80000, 120000, 200000).
      // Truoc day seed nham theo "nghin dong" (50.00) khien voucher 50k chi tru 50 dong.
      builder.HasData(
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Code = "VOUCHER_10K", Name = "Voucher 10k", Description = "Giảm 10,000đ", PointsCost = 50, DiscountAmount = 10000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Code = "VOUCHER_20K", Name = "Voucher 20k", Description = "Giảm 20,000đ", PointsCost = 100, DiscountAmount = 20000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Code = "VOUCHER_50K", Name = "Voucher 50k", Description = "Giảm 50,000đ", PointsCost = 250, DiscountAmount = 50000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Code = "VOUCHER_100K", Name = "Voucher 100k", Description = "Giảm 100,000đ", PointsCost = 500, DiscountAmount = 100000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Code = "VOUCHER_150K", Name = "Voucher 150k", Description = "Giảm 150,000đ", PointsCost = 750, DiscountAmount = 150000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Code = "VOUCHER_200K", Name = "Voucher 200k", Description = "Giảm 200,000đ", PointsCost = 1000, DiscountAmount = 200000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Code = "VOUCHER_250K", Name = "Voucher 250k", Description = "Giảm 250,000đ", PointsCost = 1250, DiscountAmount = 250000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Code = "VOUCHER_300K", Name = "Voucher 300k", Description = "Giảm 300,000đ", PointsCost = 1500, DiscountAmount = 300000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000009"), Code = "VOUCHER_400K", Name = "Voucher 400k", Description = "Giảm 400,000đ", PointsCost = 2000, DiscountAmount = 400000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true },
            new Reward { Id = Guid.Parse("10000000-0000-0000-0000-000000000010"), Code = "VOUCHER_500K", Name = "Voucher 500k", Description = "Giảm 500,000đ", PointsCost = 2500, DiscountAmount = 500000.00m, StartDate = new DateOnly(2026, 1, 1), EndDate = new DateOnly(2026, 12, 31), Status = true }
        );
    }
}
