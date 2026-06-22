using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TierPromotionConfiguration : IEntityTypeConfiguration<TierPromotion>
{
    public void Configure(EntityTypeBuilder<TierPromotion> builder)
    {
        builder.HasKey(x => x.TierPromotionId);

        builder.HasOne(x => x.Tier)
            .WithMany()
            .HasForeignKey(x => x.TierId);

        builder.HasOne(x => x.Promotion)
            .WithMany()
            .HasForeignKey(x => x.PromotionId);
    }
}