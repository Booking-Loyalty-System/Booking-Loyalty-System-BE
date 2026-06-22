using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
{
    public void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.StartTime)
            .IsRequired();

        // CẤU HÌNH MỚI: 1 TimeSlot có thể xuất hiện ở nhiều BranchTimeSlots
        builder.HasMany(ts => ts.BranchTimeSlots)
            .WithOne(bts => bts.TimeSlot)
            .HasForeignKey(bts => bts.TimeSlotId)
            .OnDelete(DeleteBehavior.Cascade);

        var staticTimeSlots = new List<TimeSlot>();
        for (int i = 8; i < 18; i++)
        {
            staticTimeSlots.Add(new TimeSlot
            {
                Id = Guid.Parse($"00000000-0000-0000-0000-0000000000{i:D2}"), 
                StartTime = new TimeOnly(i, 0),
            });
        }

        builder.HasData(staticTimeSlots);
    }
}