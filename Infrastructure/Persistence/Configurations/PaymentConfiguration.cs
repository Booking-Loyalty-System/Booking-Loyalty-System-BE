using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Method).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Gateway).IsRequired().HasMaxLength(20);
        builder.Property(p => p.TransactionRef).IsRequired().HasMaxLength(64);
        builder.Property(p => p.TransactionNo).HasMaxLength(64);
        builder.Property(p => p.BankCode).HasMaxLength(20);
        builder.Property(p => p.ResponseCode).HasMaxLength(10);
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);

        // vnp_TxnRef must be globally unique so an IPN can resolve exactly one attempt.
        builder.HasIndex(p => p.TransactionRef).IsUnique();

        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
