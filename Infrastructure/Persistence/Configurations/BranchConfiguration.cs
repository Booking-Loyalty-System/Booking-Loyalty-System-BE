using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BranchName).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Address).IsRequired().HasMaxLength(255);
        builder.Property(b => b.Hotline).HasMaxLength(20);

        // Cấu hình quan hệ: 1 Branch có nhiều WashBays
        builder.HasMany(b => b.WashBays)
            .WithOne(wb => wb.Branch)
            .HasForeignKey(wb => wb.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}