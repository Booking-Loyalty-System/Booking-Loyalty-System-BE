using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
{
    public void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Composite index for efficient queries
        builder.HasIndex(ts => new { ts.WashBayId, ts.Date, ts.StartTime });

        // Foreign keys
        builder.HasOne(ts => ts.WashBay)
            .WithMany(wb => wb.TimeSlots)
            .HasForeignKey(ts => ts.WashBayId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ts => ts.Booking)
            .WithOne(b => b.TimeSlot)
            .HasForeignKey<TimeSlot>(ts => ts.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
