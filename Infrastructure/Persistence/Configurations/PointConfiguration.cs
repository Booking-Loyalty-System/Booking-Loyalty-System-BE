using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PointConfiguration : IEntityTypeConfiguration<Point>
{
    public void Configure(EntityTypeBuilder<Point> builder)
    {
        builder.HasKey(p => p.Id);

        // One point balance per user.
        builder.HasIndex(p => p.UserId).IsUnique();

        builder.HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Point>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // DEMO XUỐNG HẠNG: điểm lũy kế cao cho customer Diamond (cus4@system.com).
        // Lũy kế 20.000 >= Min Diamond (15.000) nên KHÔNG bị rớt vì thiếu lũy kế;
        // vì chưa có điểm Earn trong tháng này, chỉ cần checkout 1 gói rẻ là rớt hạng do thiếu MaintenancePoints.
        builder.HasData(
            new Point
            {
                Id = Guid.Parse("99999999-0000-0000-0000-000000000005"),
                UserId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), // Downgrade Demo customer
                AvailablePoints = 20000,
                TotalPoints = 20000,
                UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
