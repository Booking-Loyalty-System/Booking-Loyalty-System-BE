using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PointConfiguration : IEntityTypeConfiguration<Point>
{
    public void Configure(EntityTypeBuilder<Point> builder)
    {
        builder.HasKey(p => p.Id);

        // One point balance per user.
        builder.HasIndex(p => p.UserId).IsUnique();

        builder.HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Point>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
