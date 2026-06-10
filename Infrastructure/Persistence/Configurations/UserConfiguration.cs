using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(256);

        builder.HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Use pre-hashed passwords and fixed dates to avoid generating new migrations every time
        builder.HasData(
            new User
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Email = "admin@system.com",
                PasswordHash = "$2a$11$HnjuO0ucpQhF3sNjkTBLxeWhtSN0LKUCMlMRCR4LbKsOnU9wBu06O", // Admin@123
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Email = "staff@system.com",
                PasswordHash = "$2a$11$5NGg5Rn3.ZrMWlsdr8sZEe4.dYWfCEU6hrZn2DYSctG/JJq4RhAJK", // Staff@123
                Role = UserRole.Staff,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Email = "customer@system.com",
                PasswordHash = "$2a$11$Cg0ilLXKkJHOGi/bezG1V.UAIBDHwBGVsE/k9qNiNMIkYUgVkX6ty", // customer
                Role = UserRole.Customer,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
