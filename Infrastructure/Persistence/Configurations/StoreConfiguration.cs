using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(s => s.Bookings)
            .WithOne(b => b.Store)
            .HasForeignKey(b => b.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        builder.HasData(
            new Store
            {
                Id = Guid.Parse("c1000001-0000-0000-0000-000000000001"),
                Name = "AutoWash Pro - Main Branch",
                Address = "123 Clean Street, Wash City, WC 12345",
                City = "Wash City",
                OpenTime = new TimeOnly(8, 0),
                CloseTime = new TimeOnly(18, 0),
                SlotCapacity = 4,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
