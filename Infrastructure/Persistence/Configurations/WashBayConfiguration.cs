using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class WashBayConfiguration : IEntityTypeConfiguration<WashBay>
{
    public void Configure(EntityTypeBuilder<WashBay> builder)
    {
        builder.HasKey(wb => wb.Id);

        builder.Property(wb => wb.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(wb => wb.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(wb => wb.SupportedTypes)
            .IsRequired()
            .HasMaxLength(100);

        // Seed data
        builder.HasData(
            new WashBay
            {
                Id = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000001"),
                Name = "Bay A1",
                Status = WashBayStatus.Available,
                SupportedTypes = "Small,Medium,Large",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new WashBay
            {
                Id = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000002"),
                Name = "Bay A2",
                Status = WashBayStatus.Available,
                SupportedTypes = "Small,Medium",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new WashBay
            {
                Id = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000003"),
                Name = "Bay B1",
                Status = WashBayStatus.Available,
                SupportedTypes = "Small,Medium,Large",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
