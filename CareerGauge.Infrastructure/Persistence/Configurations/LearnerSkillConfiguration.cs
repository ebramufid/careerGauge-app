using CareerGauge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerGauge.Infrastructure.Persistence.Configurations;

public class LearnerSkillConfiguration
    : IEntityTypeConfiguration<LearnerSkill>
{
    public void Configure(EntityTypeBuilder<LearnerSkill> builder)
    {
        builder.ToTable("LearnerSkills");

        builder.HasKey(ls => ls.Id);

        builder.Property(ls => ls.CurrentLevel)
            .IsRequired();

        builder.HasIndex(ls => new
        {
            ls.LearnerId,
            ls.SkillId
        })
        .IsUnique();

        builder.HasOne(ls => ls.Learner)
            .WithMany(l => l.LearnerSkills)
            .HasForeignKey(ls => ls.LearnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ls => ls.Skill)
            .WithMany(s => s.LearnerSkills)
            .HasForeignKey(ls => ls.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}