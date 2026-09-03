using CareerGauge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerGauge.Infrastructure.Persistence.Configurations;

public class LearnerConfiguration : IEntityTypeConfiguration<Learner>
{
    public void Configure(EntityTypeBuilder<Learner> builder)
    {
        builder.ToTable("Learners");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(l => l.Email)
            .HasMaxLength(255);

        builder.HasIndex(l => l.Email)
            .IsUnique()
            .HasFilter("\"Email\" IS NOT NULL");

        builder.HasMany(l => l.LearnerSkills)
            .WithOne(ls => ls.Learner)
            .HasForeignKey(ls => ls.LearnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}