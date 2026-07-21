using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RewardRedemptionConfiguration : IEntityTypeConfiguration<RewardRedemption>
{
    public void Configure(EntityTypeBuilder<RewardRedemption> builder)
    {
        builder.ToTable("RewardRedemptions");
        builder.HasKey(rr => rr.Id);

        builder.Property(rr => rr.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(rr => rr.Customer)
            .WithMany()
            .HasForeignKey(rr => rr.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rr => rr.Reward)
            .WithMany(r => r.Redemptions)
            .HasForeignKey(rr => rr.RewardId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rr => rr.Booking)
            .WithMany()
            .HasForeignKey(rr => rr.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
