using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class EmailVerificationConfiguration : IEntityTypeConfiguration<EmailVerification>
    {
        public void Configure(EntityTypeBuilder<EmailVerification> builder)
        {
            builder.HasKey(ev => ev.Id);

            builder.Property(ev => ev.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(ev => ev.VerificationToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(ev => ev.ExpiresAt)
                .IsRequired();

            builder.Property(ev => ev.IsUsed)
                .HasDefaultValue(false);

            builder.Property(ev => ev.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(ev => ev.User)
                .WithMany(u => u.EmailVerifications)
                .HasForeignKey(ev => ev.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
