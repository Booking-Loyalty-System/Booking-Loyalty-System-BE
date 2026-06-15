using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities; // Thay bằng namespace thực tế chứa class Notification của ông

namespace Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(n => n.Message)
            .IsRequired();

        builder.Property(n => n.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.IsRead)
            .HasDefaultValue(false);

        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications) 
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}