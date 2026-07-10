using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BookingImageConfiguration : IEntityTypeConfiguration<BookingImage>
{
    public void Configure(EntityTypeBuilder<BookingImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ImageUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(i => i.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(i => i.Note)
            .HasMaxLength(500);

        builder.HasOne(i => i.Booking)
            .WithMany(b => b.BookingImages)
            .HasForeignKey(i => i.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.UploadedByStaff)
            .WithMany()
            .HasForeignKey(i => i.UploadedByStaffId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
