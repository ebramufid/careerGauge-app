using CareerGauge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerGauge.Infrastructure.Persistence.Configurations;

public class AssessmentQuestionConfiguration
    : IEntityTypeConfiguration<AssessmentQuestion>
{
    public void Configure(
        EntityTypeBuilder<AssessmentQuestion> builder)
    {
        builder.ToTable("AssessmentQuestions");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.QuestionText)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(q => q.OptionA)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(q => q.OptionB)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(q => q.OptionC)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(q => q.OptionD)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(q => q.CorrectAnswer)
            .IsRequired()
            .HasMaxLength(1);

        builder.Property(q => q.Difficulty)
            .IsRequired();

        builder.HasOne(q => q.Skill)
            .WithMany(s => s.AssessmentQuestions)
            .HasForeignKey(q => q.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(q => q.SkillId);
    }
}