using CareerGauge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerGauge.Infrastructure.Persistence.Configurations;

public class AssessmentAttemptConfiguration
    : IEntityTypeConfiguration<AssessmentAttempt>
{
    public void Configure(
        EntityTypeBuilder<AssessmentAttempt> builder)
    {
        builder.ToTable("AssessmentAttempts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Score)
            .IsRequired();

        builder.Property(a => a.ResultLevel)
            .IsRequired();

        builder.Property(a => a.CompletedAt)
            .IsRequired();

        builder.HasOne(a => a.Learner)
            .WithMany(l => l.AssessmentAttempts)
            .HasForeignKey(a => a.LearnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Skill)
            .WithMany(s => s.AssessmentAttempts)
            .HasForeignKey(a => a.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => new
        {
            a.LearnerId,
            a.SkillId
        });
    }
}