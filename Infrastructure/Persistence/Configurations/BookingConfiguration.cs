using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        // Khóa chính
        builder.HasKey(b => b.Id);

        // Cấu hình các thuộc tính cơ bản
        builder.Property(b => b.BookingCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(b => b.BookingCode)
            .IsUnique();

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(b => b.TotalPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.CancellationReason)
            .HasMaxLength(500);

        builder.Property(b => b.QrData)
            .HasColumnType("text");

        builder.Property(b => b.CustomerNote)
            .HasMaxLength(500);

        // --- CẤU HÌNH QUAN HỆ KHÓA NGOẠI ---

        // 1. Quan hệ chặt chẽ với bảng trung gian BranchTimeSlot
        builder.HasOne(b => b.BranchTimeSlot)
            .WithMany(bts => bts.Bookings) // 1 BranchTimeSlot có nhiều Bookings
            .HasForeignKey(b => b.BranchTimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);

        // 2. Quan hệ tùy chọn (Nullable) với WashBay - Để Staff kéo thả xe vào bệ
        builder.HasOne(b => b.WashBay)
            .WithMany(wb => wb.Bookings) 
            .HasForeignKey(b => b.BayId)
            .OnDelete(DeleteBehavior.SetNull); // Nếu WashBay bị xóa, Booking không bị xóa, chỉ rớt WashBayId về null

        // 3. Các quan hệ 1-Nhiều khác giữ nguyên
        builder.HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Vehicle)
            .WithMany(v => v.Bookings)
            .HasForeignKey(b => b.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.WashPackage)
            .WithMany(wp => wp.Bookings)
            .HasForeignKey(b => b.WashPackageId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional promotion applied to the booking.
        builder.Property(b => b.DiscountAmount)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.Promotion)
            .WithMany()
            .HasForeignKey(b => b.PromotionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}