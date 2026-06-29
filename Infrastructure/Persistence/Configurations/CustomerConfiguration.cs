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

        builder.Property(c => c.CurrentCycleWashes)
            .HasDefaultValue(0);

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
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                FullName = "Customer Bronze Tier",
                PhoneNumber = "0901234569",
                TierId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                TotalSpent = 0m,
                TotalWashes = 0,
                IsPhoneNumberVerified = false,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                FullName = "Customer Silver Tier",
                PhoneNumber = "0901234568",
                TierId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                TotalSpent = 500m, // Giả định đã tiêu một ít
                TotalWashes = 5,
                IsPhoneNumberVerified = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                FullName = "Customer Gold Tier",
                PhoneNumber = "0901234567",
                TierId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                TotalSpent = 2500m,
                TotalWashes = 15,
                IsPhoneNumberVerified = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                FullName = "Customer Diamond Tier",
                PhoneNumber = "0901234566",
                TierId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                TotalSpent = 7000m,
                TotalWashes = 40,
                IsPhoneNumberVerified = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
