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

        builder.HasData(new Vehicle
        {
            Id = Guid.Parse("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128"),
            CustomerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            LicensePlate = "29A-88888",
            Type = VehicleType.Small,
            IsPrimary = true,
            VehicleName = "Toyota Camry 2024",
            Brand = "Toyota",
            Model = "Camry",
            Color = "Black",
        });
    }
}
