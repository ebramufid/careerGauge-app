using CareerGauge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerGauge.Infrastructure.Persistence.Configurations;

public class CareerSkillRequirementConfiguration
    : IEntityTypeConfiguration<CareerSkillRequirement>
{
    public void Configure(EntityTypeBuilder<CareerSkillRequirement> builder)
    {
        builder.ToTable("CareerSkillRequirements");

        builder.HasKey(csr => csr.Id);

        builder.Property(csr => csr.RequiredLevel)
            .IsRequired();

        builder.HasIndex(csr => new
        {
            csr.CareerProfileId,
            csr.SkillId
        })
        .IsUnique();

        builder.HasOne(csr => csr.CareerProfile)
            .WithMany(cp => cp.SkillRequirements)
            .HasForeignKey(csr => csr.CareerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(csr => csr.Skill)
            .WithMany(s => s.CareerSkillRequirements)
            .HasForeignKey(csr => csr.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}