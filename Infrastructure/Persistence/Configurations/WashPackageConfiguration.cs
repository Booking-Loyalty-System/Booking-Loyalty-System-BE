using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class WashPackageConfiguration : IEntityTypeConfiguration<WashPackage>
{
    public void Configure(EntityTypeBuilder<WashPackage> builder)
    {
        builder.HasKey(wp => wp.Id);

        builder.Property(wp => wp.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(wp => wp.Description)
            .HasMaxLength(500);

        builder.Property(wp => wp.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(wp => wp.Features)
            .HasMaxLength(2000);

        builder.Property(wp => wp.VehicleType)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Seed data
        builder.HasData(
            new WashPackage
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001"),
                Name = "Basic Wash",
                Description = "Exterior wash with basic cleaning",
                Price = 80000m,
                DurationMinutes = 20,
                Features = "[\"Exterior wash\",\"Tire cleaning\",\"Window cleaning\"]",
                VehicleType = null,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new WashPackage
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002"),
                Name = "Premium Wash",
                Description = "Full interior and exterior cleaning",
                Price = 120000m,
                DurationMinutes = 30,
                Features = "[\"Exterior wash\",\"Interior vacuum\",\"Dashboard polish\",\"Tire shine\",\"Air freshener\"]",
                VehicleType = null,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new WashPackage
            {
                Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000003"),
                Name = "VIP Detailing",
                Description = "Complete detailing with wax coating and leather care",
                Price = 200000m,
                DurationMinutes = 45,
                Features = "[\"Full exterior wash\",\"Interior deep clean\",\"Wax coating\",\"Leather conditioning\",\"Engine bay cleaning\",\"Ceramic spray\"]",
                VehicleType = null,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
