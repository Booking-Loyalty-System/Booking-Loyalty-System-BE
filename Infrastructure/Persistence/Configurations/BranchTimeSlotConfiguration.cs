using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BranchTimeSlotConfiguration : IEntityTypeConfiguration<BranchTimeSlot>
{
    public void Configure(EntityTypeBuilder<BranchTimeSlot> builder)
    {
        // Khóa chính
        builder.HasKey(bts => bts.Id);

        // Khống chế Unique: 1 Chi nhánh chỉ được có 1 bản ghi duy nhất cho 1 Khung giờ cụ thể
        builder.HasIndex(bts => new { bts.BranchId, bts.TimeSlotId })
            .IsUnique();

        // Cấu hình thuộc tính
        builder.Property(bts => bts.MaxCapacity)
            .IsRequired()
            .HasDefaultValue(1); // Set giá trị mặc định sức chứa

        builder.Property(bts => bts.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Thiết lập các khóa ngoại (đã được cấu hình ở bên kia nhưng khai báo tường minh ở đây cho chắc chắn)
        builder.HasOne(bts => bts.Branch)
            .WithMany(b => b.BranchTimeSlots)
            .HasForeignKey(bts => bts.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bts => bts.TimeSlot)
            .WithMany(ts => ts.BranchTimeSlots)
            .HasForeignKey(bts => bts.TimeSlotId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}