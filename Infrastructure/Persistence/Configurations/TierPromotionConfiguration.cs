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
            .WithMany(t => t.TierPromotions)
            .HasForeignKey(x => x.TierId);

        builder.HasOne(x => x.Promotion)
            .WithMany(p => p.TierPromotions)
            .HasForeignKey(x => x.PromotionId);
        
        builder.HasData(
            new TierPromotion { TierPromotionId = Guid.Parse("d0000000-0000-0000-0000-000000000001"), PromotionId = Guid.Parse("c0000000-0000-0000-0000-000000000004"), TierId = Guid.Parse("11111111-1111-1111-1111-111111111111") }, // Bronze
            new TierPromotion { TierPromotionId = Guid.Parse("d0000000-0000-0000-0000-000000000002"), PromotionId = Guid.Parse("c0000000-0000-0000-0000-000000000005"), TierId = Guid.Parse("22222222-2222-2222-2222-222222222222") }, // Silver
            new TierPromotion { TierPromotionId = Guid.Parse("d0000000-0000-0000-0000-000000000003"), PromotionId = Guid.Parse("c0000000-0000-0000-0000-000000000006"), TierId = Guid.Parse("33333333-3333-3333-3333-333333333333") }, // Gold
            new TierPromotion { TierPromotionId = Guid.Parse("d0000000-0000-0000-0000-000000000004"), PromotionId = Guid.Parse("c0000000-0000-0000-0000-000000000007"), TierId = Guid.Parse("44444444-4444-4444-4444-444444444444") }  // Diamond
        );
    }
}