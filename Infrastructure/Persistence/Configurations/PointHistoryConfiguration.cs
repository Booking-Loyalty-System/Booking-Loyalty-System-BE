using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PointHistoryConfiguration : IEntityTypeConfiguration<PointHistory>
{
    public void Configure(EntityTypeBuilder<PointHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.TransactionType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(h => h.Description)
            .HasMaxLength(500);

        // History lookups by balance, newest first.
        builder.HasIndex(h => new { h.PointId, h.CreatedAt });
        builder.HasIndex(h => h.BookingId);
        builder.HasIndex(h => h.RewardId);

        builder.HasOne(h => h.Point)
            .WithMany(p => p.Histories)
            .HasForeignKey(h => h.PointId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.Booking)
            .WithMany()
            .HasForeignKey(h => h.BookingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(h => h.Reward)
            .WithMany()
            .HasForeignKey(h => h.RewardId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
