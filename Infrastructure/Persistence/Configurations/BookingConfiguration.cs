using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

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
            .HasMaxLength(1000);

        // Foreign keys
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
    }
}
