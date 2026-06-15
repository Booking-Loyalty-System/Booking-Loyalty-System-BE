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
            .WithMany() 
            .HasForeignKey(pb => pb.PromotionId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(pb => pb.Branch)
            .WithMany() 
            .HasForeignKey(pb => pb.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}