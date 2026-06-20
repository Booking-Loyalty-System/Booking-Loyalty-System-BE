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
        
        // Đặt đoạn này bên trong UserConfiguration nếu ông có file riêng, hoặc cấu hình trực tiếp bằng Fluent API
        builder.HasOne(u => u.Staff)
            .WithOne(s => s.User)
            .HasForeignKey<Staff>(s => s.UserId);
        
        builder.HasData(
            new User
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Email = "admin@system.com",
                // Mật khẩu là "Admin@123" đã hash bằng BCrypt
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Email = "staff@system.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("staff"),
                Role = UserRole.Staff,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                Email = "staff1@system.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("staff"),
                Role = UserRole.Staff,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                Email = "staff2@system.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("staff"),
                Role = UserRole.Staff,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Email = "customer@system.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer"),
                Role = UserRole.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
