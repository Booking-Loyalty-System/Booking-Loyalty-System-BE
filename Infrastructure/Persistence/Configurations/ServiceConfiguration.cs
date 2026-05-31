using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.BasePrice)
            .HasColumnType("decimal(18,2)");

        builder.HasMany(s => s.Features)
            .WithOne(f => f.Service)
            .HasForeignKey(f => f.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        var basicWashId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567801");
        var premiumWashId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567802");
        var ceramicWashId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567803");

        builder.HasData(
            new Service
            {
                Id = basicWashId,
                Name = "Basic Wash",
                Description = "A thorough exterior wash with basic interior cleaning to keep your car fresh and clean.",
                BasePrice = 50000m,
                DurationMinutes = 30,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Service
            {
                Id = premiumWashId,
                Name = "Premium Wash",
                Description = "Complete interior and exterior detailing with wax application for a showroom finish.",
                BasePrice = 100000m,
                DurationMinutes = 60,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Service
            {
                Id = ceramicWashId,
                Name = "Ceramic Wash",
                Description = "Our premium service with ceramic coating and paint protection for long-lasting shine.",
                BasePrice = 200000m,
                DurationMinutes = 90,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
