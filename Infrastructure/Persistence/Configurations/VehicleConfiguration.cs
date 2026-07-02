using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);

        // Filtered unique index: license plate unique among non-deleted vehicles
        builder.HasIndex(v => v.LicensePlate)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.Property(v => v.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(v => v.VehicleName)
            .HasMaxLength(100);

        builder.Property(v => v.Color)
            .HasMaxLength(30);

        builder.Property(v => v.Brand)
            .HasMaxLength(50);

        builder.Property(v => v.Model)
            .HasMaxLength(50);

        // DEMO XUỐNG HẠNG: xe sẵn cho customer Diamond (cus4@system.com) để đặt lịch test ngay.
        builder.HasData(
            new Vehicle
            {
                Id = Guid.Parse("99999999-1111-1111-1111-000000000005"),
                CustomerId = Guid.Parse("eeeeeeee-1111-1111-1111-111111111111"), // Downgrade Demo customer
                LicensePlate = "51D-99999",
                Type = VehicleType.Medium,
                IsPrimary = true,
                VehicleName = "Demo Downgrade Car",
                Brand = "Toyota",
                Model = "2024",
                Color = "White",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
