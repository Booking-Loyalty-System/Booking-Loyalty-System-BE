using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Message)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(m => m.SenderType)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(m => m.ChatSession)
                .WithMany(s => s.ChatMessages)
                .HasForeignKey(m => m.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Customer)
                .WithMany()
                .HasForeignKey(m => m.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Staff)
                .WithMany()
                .HasForeignKey(m => m.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.CreatedAt).IsRequired();
        }
    }
}
