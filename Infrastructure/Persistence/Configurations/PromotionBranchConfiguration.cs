using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PromotionBranchConfiguration : IEntityTypeConfiguration<PromotionBranch>
{
    public void Configure(EntityTypeBuilder<PromotionBranch> builder)
    {
        builder.HasKey(pb => pb.Id);

        builder.Property(pb => pb.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(pb => pb.Promotion)
            .WithMany(p => p.PromotionBranches) 
            .HasForeignKey(pb => pb.PromotionId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(pb => pb.Branch)
            .WithMany(b => b.PromotionBranches) 
            .HasForeignKey(pb => pb.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(
            new PromotionBranch { Id = Guid.Parse("e0000000-0000-0000-0000-000000000001"), PromotionId = Guid.Parse("c0000000-0000-0000-0000-000000000001"), BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"), IsActive = true }, // Tân Bình Branch
            new PromotionBranch { Id = Guid.Parse("e0000000-0000-0000-0000-000000000002"), PromotionId = Guid.Parse("c0000000-0000-0000-0000-000000000002"), BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), IsActive = true }, // Quận 3 Branch
            new PromotionBranch { Id = Guid.Parse("e0000000-0000-0000-0000-000000000003"), PromotionId = Guid.Parse("c0000000-0000-0000-0000-000000000003"), BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), IsActive = true }  // Quận 9 Branch
        );
    }
}