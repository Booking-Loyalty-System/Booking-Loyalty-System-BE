using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PointHistoryConfiguration : IEntityTypeConfiguration<PointHistory>
{
    public void Configure(EntityTypeBuilder<PointHistory> builder)
    {
        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ph => ph.ExpiryDate)
            .IsRequired();

        builder.HasOne(ph => ph.Customer)
            .WithMany() 
            .HasForeignKey(ph => ph.CustomerId)
            .OnDelete(DeleteBehavior.Restrict); 
        
        builder.HasOne(ph => ph.Reward)
            .WithMany()
            .HasForeignKey(ph => ph.RewardId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}