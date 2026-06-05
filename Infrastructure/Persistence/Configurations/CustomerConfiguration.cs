using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(15);

        builder.HasIndex(c => c.PhoneNumber).IsUnique();

        builder.Property(c => c.TotalSpent)
            .HasColumnType("decimal(18,2)");

        builder.HasMany(c => c.Vehicles)
            .WithOne(v => v.Customer)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(c => c.Tier)
            .WithMany(t => t.Customers)
            .HasForeignKey(c => c.TierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
