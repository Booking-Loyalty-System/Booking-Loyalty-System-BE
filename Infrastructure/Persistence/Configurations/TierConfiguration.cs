using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TierConfiguration : IEntityTypeConfiguration<Tier>
{
    public void Configure(EntityTypeBuilder<Tier> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.TierName)
            .IsRequired()
            .HasMaxLength(50); 

        builder.Property(b => b.PointRate)
            .HasColumnType("decimal(18,2)");

        builder.HasMany(t => t.Customers)
            .WithOne(c => c.Tier)
            .HasForeignKey(c => c.TierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Tier
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                TierName = "Bronze",
                PointRate = 1.00m,
                BookingWindow = 7,
                Level = PriorityLevel.Bronze,
                MinPointsRequired = 0,
                MaintenancePoints = 0
            },
            new Tier
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                TierName = "Silver",
                PointRate = 1.50m,
                BookingWindow = 14,
                Level = PriorityLevel.Silver,
                MinPointsRequired = 2000,
                MaintenancePoints = 300
            },
            new Tier
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                TierName = "Gold",
                PointRate = 2.00m,
                BookingWindow = 21,
                Level = PriorityLevel.Gold,
                MinPointsRequired = 6000,
                MaintenancePoints = 1000
            },
            new Tier
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                TierName = "Diamond",
                PointRate = 3.00m,
                BookingWindow = 30,
                Level = PriorityLevel.Diamond,
                MinPointsRequired = 15000,
                MaintenancePoints = 3000
            }
        );
    }
}