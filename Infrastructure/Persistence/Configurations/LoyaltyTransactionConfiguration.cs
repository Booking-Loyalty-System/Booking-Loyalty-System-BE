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

        // Index hỗ trợ tìm kiếm lịch sử theo khách hàng, sắp xếp mới nhất lên đầu
        builder.HasIndex(lt => new { lt.CustomerId, lt.CreatedAt });

        // Hỗ trợ truy vấn nhanh các giao dịch liên quan đến Booking hoặc Reward
        builder.HasIndex(lt => lt.BookingId);
        builder.HasIndex(lt => lt.RewardId);

        // Quan hệ với Customer
        builder.HasOne(lt => lt.Customer)
            .WithMany()
            .HasForeignKey(lt => lt.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ với Booking
        builder.HasOne(lt => lt.Booking)
            .WithMany()
            .HasForeignKey(lt => lt.BookingId)
            .OnDelete(DeleteBehavior.SetNull);

        // Quan hệ với Reward (mới thêm vào sau khi gộp)
        builder.HasOne(lt => lt.Reward)
            .WithMany()
            .HasForeignKey(lt => lt.RewardId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}