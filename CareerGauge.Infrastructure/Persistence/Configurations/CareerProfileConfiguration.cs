using CareerGauge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerGauge.Infrastructure.Persistence.Configurations;

public class CareerProfileConfiguration
    : IEntityTypeConfiguration<CareerProfile>
{
    public void Configure(EntityTypeBuilder<CareerProfile> builder)
    {
        builder.ToTable("CareerProfiles");

        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(cp => cp.Description)
            .HasMaxLength(1000);

        builder.HasIndex(cp => cp.Name)
            .IsUnique();
    }
}