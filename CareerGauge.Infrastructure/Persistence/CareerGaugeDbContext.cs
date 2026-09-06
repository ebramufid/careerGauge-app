using CareerGauge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerGauge.Infrastructure.Persistence;

public class CareerGaugeDbContext : DbContext
{
    public CareerGaugeDbContext(
        DbContextOptions<CareerGaugeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Learner> Learners => Set<Learner>();

    public DbSet<Skill> Skills => Set<Skill>();

    public DbSet<LearnerSkill> LearnerSkills => Set<LearnerSkill>();

    public DbSet<CareerProfile> CareerProfiles => Set<CareerProfile>();

    public DbSet<CareerSkillRequirement> CareerSkillRequirements =>
        Set<CareerSkillRequirement>();

    public DbSet<AssessmentQuestion> AssessmentQuestions =>
        Set<AssessmentQuestion>();

    public DbSet<AssessmentAttempt> AssessmentAttempts =>
        Set<AssessmentAttempt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CareerGaugeDbContext).Assembly);
    }
}