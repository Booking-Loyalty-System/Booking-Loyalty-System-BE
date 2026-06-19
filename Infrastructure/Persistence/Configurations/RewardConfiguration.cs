using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RewardConfiguration : IEntityTypeConfiguration<Reward>
{
    public void Configure(EntityTypeBuilder<Reward> builder)
    {
        builder.ToTable("Rewards");

        // Khóa chính
        builder.HasKey(r => r.Id);

        // Cấu hình các thuộc tính thời gian
        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.EndDate)
            .IsRequired();
        
        builder.Property(r => r.Status)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasMany(r => r.Bookings)
            .WithOne(b => b.Reward) 
            .HasForeignKey(b => b.RewardId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}
