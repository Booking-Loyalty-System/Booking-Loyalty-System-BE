using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ServiceFeatureConfiguration : IEntityTypeConfiguration<ServiceFeature>
{
    public void Configure(EntityTypeBuilder<ServiceFeature> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.FeatureDescription)
            .IsRequired()
            .HasMaxLength(200);

        // Seed data
        var basicWashId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567801");
        var premiumWashId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567802");
        var ceramicWashId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567803");

        builder.HasData(
            // Basic Wash features
            new ServiceFeature { Id = Guid.Parse("b1000001-0000-0000-0000-000000000001"), ServiceId = basicWashId, FeatureDescription = "Exterior wash", SortOrder = 1 },
            new ServiceFeature { Id = Guid.Parse("b1000001-0000-0000-0000-000000000002"), ServiceId = basicWashId, FeatureDescription = "Tire cleaning", SortOrder = 2 },
            new ServiceFeature { Id = Guid.Parse("b1000001-0000-0000-0000-000000000003"), ServiceId = basicWashId, FeatureDescription = "Basic interior vacuum", SortOrder = 3 },

            // Premium Wash features
            new ServiceFeature { Id = Guid.Parse("b1000002-0000-0000-0000-000000000001"), ServiceId = premiumWashId, FeatureDescription = "Everything in Basic", SortOrder = 1 },
            new ServiceFeature { Id = Guid.Parse("b1000002-0000-0000-0000-000000000002"), ServiceId = premiumWashId, FeatureDescription = "Interior detailing", SortOrder = 2 },
            new ServiceFeature { Id = Guid.Parse("b1000002-0000-0000-0000-000000000003"), ServiceId = premiumWashId, FeatureDescription = "Wax application", SortOrder = 3 },
            new ServiceFeature { Id = Guid.Parse("b1000002-0000-0000-0000-000000000004"), ServiceId = premiumWashId, FeatureDescription = "Dashboard polish", SortOrder = 4 },

            // Ceramic Wash features
            new ServiceFeature { Id = Guid.Parse("b1000003-0000-0000-0000-000000000001"), ServiceId = ceramicWashId, FeatureDescription = "Everything in Premium", SortOrder = 1 },
            new ServiceFeature { Id = Guid.Parse("b1000003-0000-0000-0000-000000000002"), ServiceId = ceramicWashId, FeatureDescription = "Ceramic coating", SortOrder = 2 },
            new ServiceFeature { Id = Guid.Parse("b1000003-0000-0000-0000-000000000003"), ServiceId = ceramicWashId, FeatureDescription = "Paint protection", SortOrder = 3 },
            new ServiceFeature { Id = Guid.Parse("b1000003-0000-0000-0000-000000000004"), ServiceId = ceramicWashId, FeatureDescription = "Premium fragrance", SortOrder = 4 }
        );
    }
}
