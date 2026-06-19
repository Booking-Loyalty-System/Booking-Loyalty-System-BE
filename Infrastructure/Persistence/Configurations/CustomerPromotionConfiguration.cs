using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class CustomerPromotionConfiguration : IEntityTypeConfiguration<CustomerPromotion>
{
    public void Configure(EntityTypeBuilder<CustomerPromotion> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsUsed).HasDefaultValue(false);
        
        builder.HasOne(x => x.Customer)
            .WithMany() // Nếu Customer có collection CustomerPromotions thì điền vào đây
            .HasForeignKey(x => x.CustomerId);

        builder.HasOne(x => x.Promotion)
            .WithMany()
            .HasForeignKey(x => x.PromotionId);
    }
}