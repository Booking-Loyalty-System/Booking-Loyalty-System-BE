using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Discount)
            .HasPrecision(5, 2) 
            .IsRequired();

        builder.Property(p => p.PriorityLevel)
            .IsRequired();

        // Mối quan hệ với PromotionBranch
        builder.HasMany(p => p.PromotionBranches)
            .WithOne(pb => pb.Promotion)
            .HasForeignKey(pb => pb.PromotionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
