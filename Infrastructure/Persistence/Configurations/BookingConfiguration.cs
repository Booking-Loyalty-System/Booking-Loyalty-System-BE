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

        // 1. Quan hệ 1-1 với bảng trung gian WashBayTimeSlot (Khống chế cứng 1 thời điểm chỉ 1 xe)
        builder.HasOne(b => b.WashBayTimeSlot)
            .WithOne(wbts => wbts.Booking)
            .HasForeignKey<Booking>(b => b.WashBayTimeSlotId) // Khóa ngoại nằm ở bảng Booking
            .OnDelete(DeleteBehavior.Restrict); // Giữ lại lịch sử booking, tránh cascade xóa mất data

        // 2. Các quan hệ 1-Nhiều khác giữ nguyên
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
        
        builder.HasOne(b => b.Branch)
            .WithMany(br => br.Bookings)
            .HasForeignKey(b => b.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}