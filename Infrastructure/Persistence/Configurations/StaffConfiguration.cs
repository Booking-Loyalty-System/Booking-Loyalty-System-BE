using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(s => s.IsAvailable)
            .HasDefaultValue(true);

        // 1. Cấu hình quan hệ 1-1 với User (Mỗi Staff ứng với 1 tài khoản login)
        builder.HasOne(s => s.User)
            .WithOne(u => u.Staff)
            .HasForeignKey<Staff>(s => s.UserId) // Khóa ngoại đặt tại bảng Staff
            .OnDelete(DeleteBehavior.Cascade);   // Xóa User thì xóa Staff tương ứng

        // 2. Cấu hình quan hệ N-1 với Branch (Nhiều Staff thuộc 1 Chi nhánh)
        builder.HasOne(s => s.Branch)
            .WithMany(b => b.Staffs)
            .HasForeignKey(s => s.BranchId)
            .OnDelete(DeleteBehavior.Restrict); 
        builder.HasData(
            new Staff
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 
                BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 
                FullName = "Nguyễn Văn Staff",
                PhoneNumber = "0901234567",
                IsAvailable = true
            },
            new Staff
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"), 
                BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), 
                FullName = "Trần Thị Staff Một",
                PhoneNumber = "0907654321",
                IsAvailable = true
            },
            new Staff
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"), 
                BranchId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"),
                FullName = "Lê Hoàng Staff Hai",
                PhoneNumber = "0911223344",
                IsAvailable = true
            }
        );
    }
}