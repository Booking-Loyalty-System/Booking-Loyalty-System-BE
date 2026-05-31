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

        builder.Property(v => v.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        // Filtered unique index: license plate unique among non-deleted vehicles
        builder.HasIndex(v => v.LicensePlate)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

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
    }
}
