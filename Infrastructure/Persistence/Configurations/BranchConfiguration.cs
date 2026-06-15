using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BranchName).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Address).IsRequired().HasMaxLength(255);
        builder.Property(b => b.Hotline).HasMaxLength(20);

        // Cấu hình quan hệ: 1 Branch có nhiều WashBays
        builder.HasMany(b => b.WashBays)
            .WithOne(wb => wb.Branch)
            .HasForeignKey(wb => wb.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(
            new Branch { 
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 
                BranchName = "Quận 9 Branch", 
                Address = "625 Nguyễn Xiển, Long Bình, Hồ Chí Minh 70000, Vietnam", 
                Hotline = "0123456789", 
                OperatingHours = "8am-9pm", 
                Latitude = 10.8415798,
                Longitude = 106.8294047,
                Status = BranchStatus.Active
            },
            new Branch { 
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"),
                BranchName = "Quận 3 Branch", 
                Address = "303 Cách Mạng Tháng 8 Tổ 20, Khu phố Khu phố, Hòa Hưng, Hồ Chí Minh, Vietnam", 
                Hotline = "0987654321", 
                OperatingHours = "8am-8pm", 
                Latitude = 10.7794176, 
                Longitude = 106.678039, 
                Status = BranchStatus.Active
            },
            new Branch { 
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"),
                BranchName = "Tân Bình Branch", 
                Address = "19Bis Cộng Hòa, Bảy Hiền, Hồ Chí Minh, Vietnam", 
                Hotline = "0912345678", 
                OperatingHours = "8am-9pm", 
                Latitude = 10.8016008, 
                Longitude = 106.6482373, 
                Status = BranchStatus.Active
            }
        );
    }
}