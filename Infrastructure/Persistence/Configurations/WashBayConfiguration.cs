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
            new WashBay { Id = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000001"), Name = "Bay A1 (Q9)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000002"), Name = "Bay A2 (Q9)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000003"), Name = "Bay B1 (Q9)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium,Large", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0001-0001-0001-000000000004"), Name = "Bay B2 (Q9)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium,Large", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
            // --- WashBays cho Quận 3 ---
            new WashBay { Id = Guid.Parse("b1b2c3d4-0003-0001-0001-000000000001"), Name = "Bay A1 (Q3)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0003-0001-0001-000000000002"), Name = "Bay A2 (Q3)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0003-0001-0001-000000000003"), Name = "Bay B1 (Q3)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium,Large", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0003-0001-0001-000000000004"), Name = "Bay B2 (Q3)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium,Large", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03") },

            // --- WashBays cho Tân Bình ---
            new WashBay { Id = Guid.Parse("b1b2c3d4-0002-0001-0001-000000000001"), Name = "Bay A1 (TB)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0002-0001-0001-000000000002"), Name = "Bay A2 (TB)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0002-0001-0001-000000000003"), Name = "Bay B1 (TB)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium,Large", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02") },
            new WashBay { Id = Guid.Parse("b1b2c3d4-0002-0001-0001-000000000004"), Name = "Bay B2 (TB)", Status = WashBayStatus.Available, SupportedTypes = "Small,Medium,Large", BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02") }

        );
    }
}
