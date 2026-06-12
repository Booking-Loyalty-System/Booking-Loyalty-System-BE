using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LoyaltyTransactionConfiguration : IEntityTypeConfiguration<LoyaltyTransaction>
{
    public void Configure(EntityTypeBuilder<LoyaltyTransaction> builder)
    {
        builder.HasKey(lt => lt.Id);

        builder.Property(lt => lt.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(lt => lt.Description)
            .HasMaxLength(500);

        // Read balance/history by customer, newest first.
        builder.HasIndex(lt => new { lt.CustomerId, lt.CreatedAt });

        // One Earn row per booking is enforced in code (idempotency); index supports the lookup.
        builder.HasIndex(lt => lt.BookingId);

        builder.HasOne(lt => lt.Customer)
            .WithMany()
            .HasForeignKey(lt => lt.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lt => lt.Booking)
            .WithMany()
            .HasForeignKey(lt => lt.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
