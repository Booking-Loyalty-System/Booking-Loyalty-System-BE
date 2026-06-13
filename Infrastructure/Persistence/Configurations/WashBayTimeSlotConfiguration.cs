using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class WashBayTimeSlotConfiguration : IEntityTypeConfiguration<WashBayTimeSlot>
{
    public void Configure(EntityTypeBuilder<WashBayTimeSlot> builder)
    {
        builder.HasKey(wbts => wbts.Id);

        builder.Property(wbts => wbts.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(wbts => wbts.Date)
            .IsRequired();
        
        builder.HasIndex(wbts => new { wbts.WashBayId, wbts.Date, wbts.TimeSlotId });

        builder.HasOne(wbts => wbts.WashBay)
            .WithMany(wb => wb.WashBayTimeSlots)
            .HasForeignKey(wbts => wbts.WashBayId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(wbts => wbts.TimeSlot)
            .WithMany(ts => ts.WashBayTimeSlots)
            .HasForeignKey(wbts => wbts.TimeSlotId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wbts => wbts.Booking)
            .WithOne(b => b.WashBayTimeSlot)
            .HasForeignKey<Booking>(b => b.WashBayTimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}