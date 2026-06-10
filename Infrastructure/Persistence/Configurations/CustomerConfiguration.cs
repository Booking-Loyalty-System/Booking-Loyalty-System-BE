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
        
        builder.HasData(
            new Customer
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                FullName = "Customer User",
                PhoneNumber = "0901234567",
                TierId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                TotalSpent = 0m,
                TotalWashes = 0,
                TotalPoints = 0,
                LifetimePoints = 0,
                IsPhoneNumberVerified = false,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
