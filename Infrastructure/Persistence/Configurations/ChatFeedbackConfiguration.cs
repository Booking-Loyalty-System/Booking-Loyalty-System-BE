using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ChatFeedbackConfiguration : IEntityTypeConfiguration<ChatFeedback>
    {
        public void Configure(EntityTypeBuilder<ChatFeedback> builder)
        {
            builder.HasKey(cf => cf.Id);

            builder.Property(cf => cf.Rating)
                .IsRequired();

            builder.Property(cf => cf.Comment)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(cf => cf.CreatedAt)
                .IsRequired();

            builder.HasOne(cf => cf.ChatSession)
                .WithOne()
                .HasForeignKey<ChatFeedback>(cf => cf.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}