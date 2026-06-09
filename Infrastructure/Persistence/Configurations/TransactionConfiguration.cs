using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TransactionCode)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(x => x.TransactionCode).IsUnique();

        builder.HasOne(x => x.Booking)
            .WithMany(b => b.Transactions) 
            .HasForeignKey(x => x.BookingId);
    }
}