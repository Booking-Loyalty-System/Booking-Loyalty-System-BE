using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AddOnConfiguration : IEntityTypeConfiguration<AddOn>
{
    public void Configure(EntityTypeBuilder<AddOn> builder)
    {
        builder.ToTable("AddOns");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Description).HasMaxLength(500);
        builder.Property(a => a.Price).HasColumnType("decimal(18,2)");

        // Seed the standard add-on catalog (fixed IDs + timestamp so migrations stay deterministic).
        var seededAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        builder.HasData(
            new AddOn { Id = Guid.Parse("a1110001-0000-0000-0000-000000000001"), Name = "Phủ wax", Description = "Phủ wax bảo vệ sơn", Price = 50000m, DurationMinutes = 15, IsActive = true, CreatedAt = seededAt },
            new AddOn { Id = Guid.Parse("a1110001-0000-0000-0000-000000000002"), Name = "Vệ sinh nội thất", Description = "Hút bụi, lau sạch nội thất", Price = 80000m, DurationMinutes = 20, IsActive = true, CreatedAt = seededAt },
            new AddOn { Id = Guid.Parse("a1110001-0000-0000-0000-000000000003"), Name = "Khử mùi", Description = "Khử mùi khoang xe", Price = 30000m, DurationMinutes = 10, IsActive = true, CreatedAt = seededAt },
            new AddOn { Id = Guid.Parse("a1110001-0000-0000-0000-000000000004"), Name = "Đánh bóng đèn", Description = "Đánh bóng pha đèn ố mờ", Price = 40000m, DurationMinutes = 15, IsActive = true, CreatedAt = seededAt }
        );
    }
}
