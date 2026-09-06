namespace CareerGauge.Domain.Entities;

public class Learner
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Email { get; set; }

    public ICollection<LearnerSkill> LearnerSkills { get; set; } = [];

    public ICollection<AssessmentAttempt> AssessmentAttempts { get; set; } = [];
}