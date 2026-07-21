using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BookingAddOnConfiguration : IEntityTypeConfiguration<BookingAddOn>
{
    public void Configure(EntityTypeBuilder<BookingAddOn> builder)
    {
        builder.ToTable("BookingAddOns");
        builder.HasKey(ba => ba.Id);

        builder.Property(ba => ba.Price).HasColumnType("decimal(18,2)");

        // The same add-on cannot be attached twice to one booking.
        builder.HasIndex(ba => new { ba.BookingId, ba.AddOnId }).IsUnique();

        builder.HasOne(ba => ba.Booking)
            .WithMany(b => b.BookingAddOns)
            .HasForeignKey(ba => ba.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ba => ba.AddOn)
            .WithMany(a => a.BookingAddOns)
            .HasForeignKey(ba => ba.AddOnId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
